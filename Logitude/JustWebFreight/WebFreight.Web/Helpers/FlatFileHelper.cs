using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class FlatFileHelper
    {
        private int Tenant;
        private Tenant myTenant;
        public FlatFileHelper(int tenant)
        {
            this.Tenant = tenant;

            TenantRepository tenantRepository = new TenantRepository(tenant);
            this.myTenant = tenantRepository.GetSingleTenant(tenant);
        }

        private string buildLine(string value, int textLength, string PadDirection, char paddingChar)
        {
            string s = null;

            if (string.IsNullOrEmpty(value))
            {
                value = "";
            }

            value = value.Trim();

            if (value.Length > textLength)
            {
                value = value.Substring(0, textLength);
            }

            if (PadDirection == "L")
            {
                s = value.PadLeft(textLength, paddingChar);
            }

            else if (PadDirection == "R")
            {
                s = value.PadRight(textLength, paddingChar);
            }

            else
            {
                s = value;
            }

            return s;
        }

        #region AES        
        public void MapAESFile(string shipmentId, string fileName)
        {
            ShipmentQuery repository = new ShipmentQuery(Tenant);
            ShipmentPM myShipment = repository.GetSinglePMWithoutComposition(shipmentId, Tenant);

            if (myShipment != null)
            {
                string myResult = null;
                myResult = this.MapEntity(myShipment);

                if (!string.IsNullOrEmpty(myResult))
                {
                    StringBuilder stringbuilder = new StringBuilder();
                    Encoding encoding = new UTF8Encoding();
                    stringbuilder.AppendLine(myResult);
                    byte[] errordata = encoding.GetBytes(stringbuilder.ToString());
                    string[] fileProps = fileName.Split('.');

                    Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
                    {
                        FileName = fileProps[0],
                        Extension = fileProps.Length > 1 ? fileProps[1] : null,
                        Tenant = Tenant,
                        FileSize = errordata.Length,
                        HasExternalContainer = true,
                        ExternalContainerName = "tenant" + Tenant
                    };

                    Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
                    storageservice.Write(errordata, fileInfo);

                    ShipmentCustomsTransmissionArgs args = new ShipmentCustomsTransmissionArgs()
                    {
                        ShipmentId = shipmentId,
                        MessageType = "CBAS",
                        Status = "SENT",
                    };

                    ShipmentCustomsTransmissionHelper transmissionHelper = new ShipmentCustomsTransmissionHelper(Tenant);
                    transmissionHelper.Run(args);
                }
            }
        }
        private string MapEntity(ShipmentPM myShipment)
        {
            StringBuilder main = new StringBuilder();

            ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(Tenant);
            IQueryable<ShipmentPackage> packages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(myShipment.Id, myShipment.Tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(Tenant);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            CardRepository cardRepository = new CardRepository(commonContext);
            ContactRepository contactRepository = new ContactRepository(commonContext);

            main.Append(this.CreateString_A(myShipment));
            main.Append(this.CreateString_B(myShipment));
            main.Append(this.CreateString_SC1(myShipment));
            main.Append(this.CreateString_SC2(myShipment));
            main.Append(this.CreateString_SC3(myShipment, packages));

            if (!string.IsNullOrEmpty(myShipment.ShipperId))
            {
                Card shipperCard = cardRepository.GetSingleCard(myShipment.ShipperId, myShipment.Tenant);
                Address shipperAddress = addressRepository.GetSingleAddress(myShipment.ShipperAddressId, myShipment.Tenant);
                Contact shipperContact = contactRepository.GetSingleContact(myShipment.ShipperContactId, myShipment.Tenant);

                main.Append(this.CreateString_N01(shipperCard, shipperContact, "shipper"));
                main.Append(this.CreateString_N02(shipperAddress, shipperContact));
                main.Append(this.CreateString_N03(shipperCard, shipperAddress));
            }

            if (!string.IsNullOrEmpty(myShipment.ConsigneeId))
            {
                Card consigneeCard = cardRepository.GetSingleCard(myShipment.ConsigneeId, myShipment.Tenant);
                Address consigneeAddress = addressRepository.GetSingleAddress(myShipment.ConsigneeAddressId, myShipment.Tenant);
                Contact consigneeContact = contactRepository.GetSingleContact(myShipment.ConsigneeContactId, myShipment.Tenant);

                main.Append(this.CreateString_N01(consigneeCard, consigneeContact, "consignee"));
                main.Append(this.CreateString_N02(consigneeAddress, consigneeContact));
                main.Append(this.CreateString_N03(consigneeCard, consigneeAddress));
            }

            if (!string.IsNullOrEmpty(myShipment.IssuingCarrierAgentId))
            {
                Card agentCard = cardRepository.GetSingleCard(myShipment.IssuingCarrierAgentId, myShipment.Tenant);
                Address agentAddress = addressRepository.GetSingleAddress(myShipment.IssuingCarrierAddressId, myShipment.Tenant);

                main.Append(this.CreateString_N01(agentCard, null, "agent"));
                main.Append(this.CreateString_N02(agentAddress, null));
                main.Append(this.CreateString_N03(agentCard, agentAddress));
            }

            main.Append(this.CreateString_CL1(myShipment, packages));
            main.Append(this.CreateString_CL2(myShipment, packages));
            main.Append(this.CreateString_Z(myShipment));

            return main.ToString();
        }
        private string CreateString_A(ShipmentPM myShipment)
        {
            StringBuilder str = new StringBuilder(80);

            string date = String.Format("{0:yyyyMMdd}", DateTime.Now.Date);
            string tenantVAT = this.myTenant == null ? null : this.myTenant.VatNumber;
            int tenantNumber = this.myTenant == null ? 0 : this.myTenant.Id;

            str.Append("A");
            str.Append(this.buildLine("", 2, "R", ' '));
            str.Append(this.buildLine("", 2, "R", ' '));
            str.Append(this.buildLine(tenantVAT, 9, "R", ' '));
            str.Append(this.buildLine("", 6, "R", ' '));
            str.Append("E");
            str.Append("XP");
            str.Append(date);
            str.Append(this.buildLine(tenantNumber.ToString(), 6, "L", '0'));
            str.Append("N");
            str.Append("661661661");
            str.Append(this.buildLine("", 33, "R", ' '));

            return str.ToString();
        }
        private string CreateString_B(ShipmentPM myShipment)
        {
            StringBuilder str = new StringBuilder(80);

            string tenantVAT = this.myTenant == null ? null : this.myTenant.VatNumber;
            string tenantName = this.myTenant == null ? null : this.myTenant.Company;

            str.Append("B");
            str.Append(this.buildLine("", 2, "R", ' '));
            str.Append(this.buildLine(tenantVAT, 11, "R", ' '));
            str.Append("E");
            str.Append(this.buildLine("", 10, "R", ' '));
            str.Append(this.buildLine(tenantName, 30, "R", ' '));
            str.Append(this.buildLine("", 25, "R", ' '));

            return str.ToString();
        }
        private string CreateString_SC1(ShipmentPM myShipment)
        {
            StringBuilder str = new StringBuilder(80);
            AddressRepository addressRepository = new AddressRepository(myShipment.Tenant);
            CardRepository cardRepository = new CardRepository(myShipment.Tenant);
            ComputingPartnerTranslationHelper computingPartnerHelper = new ComputingPartnerTranslationHelper(myShipment.Tenant);

            var fromPortCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(myShipment.MainCarriageFromPortCode, "G-CBP", "Port");
            if (string.IsNullOrEmpty(fromPortCode))
            {
                fromPortCode = "";
            }

            var finalPortCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(myShipment.MainCarriageFinalDestinationPortCode, "G-CBP", "Port");
            if (string.IsNullOrEmpty(finalPortCode))
            {
                finalPortCode = "";
            }

            string transportMode = "10";
            switch (myShipment.ShipmentTypeId)
            {
                case "LCLD":
                    {
                        transportMode = "10";
                        break;
                    }

                case "FCLD":
                    {
                        transportMode = "11";
                        break;
                    }

                case "LTL":
                    {
                        transportMode = "30";
                        break;
                    }

                case "FTL":
                    {
                        transportMode = "31";
                        break;
                    }

                default:
                    {
                        transportMode = "40";
                        break;
                    }
            }

            Address shipperAddress = addressRepository.GetSingleAddress(myShipment.ShipperAddressId, myShipment.Tenant);
            string shipperStateCode = shipperAddress == null ? "" : (shipperAddress.State == null ? "" : shipperAddress.State.Code);

            string carrierCode = "";
            string vesselName = "";
            Card carrier = cardRepository.GetSingleCard(myShipment.MainCarriageCarrierId, myShipment.Tenant);
            if (carrier != null)
            {
                if (carrier.PartnerTypeId == "AL")
                {
                    carrierCode = carrier.Code;
                }

                else if (carrier.PartnerTypeId == "SL")
                {
                    carrierCode = carrier.ShippingLine == null ? "" : carrier.ShippingLine.SCACCode;
                }

                else if (carrier.PartnerTypeId == "TR")
                {
                    carrierCode = carrier.Code;
                }
            }

            if (myShipment.TransportModeId == "O")
            {
                if (!string.IsNullOrEmpty(myShipment.MainCarriageVesselId))
                {
                    vesselName = myShipment.MainCarriageVesselName;
                }
            }
            else
            {
                vesselName = myShipment.MainCarriageCarrierName;
            }

            string ETD = myShipment.MainCarriageETD == null ? "" : String.Format("{0:yyyyMMdd}", myShipment.MainCarriageETD);

            string dangerous = myShipment.IsDangerous ? "Y" : "N";

            str.Append("SC1");
            str.Append("Y");
            str.Append(transportMode);
            str.Append(this.buildLine(myShipment.MainCarriageFinalDestinationPortCountryCode, 2, "R", ' '));
            str.Append(this.buildLine(shipperStateCode, 2, "R", ' '));
            str.Append(this.buildLine(carrierCode, 4, "R", ' '));
            str.Append(this.buildLine(myShipment.ShipmentNumber, 17, "R", ' '));
            str.Append("A");
            str.Append(this.buildLine(vesselName, 23, "R", ' '));
            str.Append("3");
            str.Append("F");
            str.Append(this.buildLine(finalPortCode, 5, "R", ' '));
            str.Append(this.buildLine(fromPortCode, 4, "R", ' '));
            str.Append(ETD);
            str.Append(" ");
            str.Append(dangerous);
            str.Append(this.buildLine("", 4, "R", ' '));

            return str.ToString();
        }
        private string CreateString_SC2(ShipmentPM myShipment)
        {
            StringBuilder str = new StringBuilder(80);

            str.Append("SC2");
            str.Append("70");
            str.Append(this.buildLine("", 15, "R", ' '));
            str.Append(this.buildLine("", 7, "R", ' '));
            str.Append(this.buildLine("", 13, "R", ' '));
            str.Append("N");
            str.Append(this.buildLine("", 15, "R", ' '));
            str.Append(this.buildLine("", 24, "R", ' '));

            return str.ToString();
        }
        private string CreateString_SC3(ShipmentPM myShipment, IQueryable<ShipmentPackage> packages)
        {
            StringBuilder str = new StringBuilder(80);

            bool emptyLine = false;

            if (myShipment.ShipmentTypeId == "FTL" || myShipment.ShipmentTypeId == "FCLD")
            {
                if (packages.Count() > 0)
                {
                    foreach (ShipmentPackage item in packages)
                    {
                        str.Append("SC3");
                        str.Append(this.buildLine(item.ContainerNumber, 14, "R", ' '));
                        str.Append(this.buildLine(item.ShipperSeal, 15, "R", ' '));
                        str.Append(this.buildLine(myShipment.Master, 30, "R", ' '));
                        str.Append(this.buildLine("", 18, "R", ' '));
                    }
                }

                else
                {
                    emptyLine = true;
                }
            }

            else
            {
                emptyLine = true;
            }

            if (emptyLine)
            {
                str.Append("SC3");
                str.Append(this.buildLine("", 14, "R", ' '));
                str.Append(this.buildLine("", 15, "R", ' '));
                str.Append(this.buildLine(myShipment.Master, 30, "R", ' '));
                str.Append(this.buildLine("", 18, "R", ' '));
            }

            return str.ToString();
        }
        private string CreateString_N01(Card partner, Contact contact, string type)
        {
            StringBuilder str = new StringBuilder(80);

            string partnerType = null;
            string consignee_n = null;

            switch (type)
            {
                case "shipper":
                    {
                        partnerType = "E";
                        break;
                    }

                case "consignee":
                    {
                        partnerType = "F";
                        consignee_n = "N";
                        break;
                    }

                case "agent":
                    {
                        partnerType = "C";
                        break;
                    }
            }

            string contactFirstName = null;
            string contactLastName = null;
            if (contact != null)
            {
                string[] names = contact.EnglishName.Split(' ');
                contactFirstName = names[0];

                if (names.Length > 1)
                {
                    contactLastName = names[1];
                }
            }

            str.Append("N01");
            str.Append(this.buildLine(partner.VatNumber, 11, "R", ' '));
            str.Append("E");
            str.Append(this.buildLine(partnerType, 1, "R", ' '));
            str.Append(this.buildLine(partner.EnglishName, 30, "R", ' '));
            str.Append(this.buildLine(contactFirstName, 13, "R", ' '));
            str.Append(this.buildLine(contactLastName, 20, "R", ' '));
            str.Append(this.buildLine(consignee_n, 1, "R", ' '));

            return str.ToString();
        }
        private string CreateString_N02(Address address, Contact contact)
        {
            StringBuilder str = new StringBuilder(80);

            string phone = address == null ? null : address.PhoneNumber;

            if (contact != null)
            {
                if (!string.IsNullOrEmpty(contact.BusinessPhone))
                {
                    phone = contact.BusinessPhone;
                }
            }

            str.Append("N02");
            str.Append(this.buildLine(address == null ? null : address.Address1, 32, "R", ' '));
            str.Append(this.buildLine(address == null ? null : address.Address2, 32, "R", ' '));
            str.Append(this.buildLine(phone, 13, "R", ' '));

            return str.ToString();
        }
        private string CreateString_N03(Card partner, Address address)
        {
            StringBuilder str = new StringBuilder(80);

            string stateCode = address == null ? null : (address.State == null ? "" : address.State.Code);
            string countryCode = address == null ? null : (address.Country == null ? "" : address.Country.Code);

            str.Append("N03");
            str.Append(this.buildLine(address == null ? null : address.City, 25, "R", ' '));
            str.Append(this.buildLine(stateCode, 2, "R", ' '));
            str.Append(this.buildLine(countryCode, 2, "R", ' '));
            str.Append(this.buildLine(address == null ? null : address.ZipCode, 9, "R", ' '));
            str.Append(this.buildLine(partner.VatNumber, 9, "R", ' '));
            str.Append("E");
            str.Append("D");
            str.Append(this.buildLine("", 28, "R", ' '));

            return str.ToString();
        }
        private string CreateString_CL1(ShipmentPM myShipment, IQueryable<ShipmentPackage> packages)
        {
            StringBuilder str = new StringBuilder(80);

            int lineNumber = 1;

            if (packages.Count() > 0)
            {
                foreach (ShipmentPackage item in packages)
                {
                    str.Append("CL1");
                    str.Append("OS");
                    str.Append(" ");
                    str.Append(this.buildLine(lineNumber++.ToString(), 4, "L", '0'));
                    str.Append(this.buildLine(item.Description, 45, "R", ' '));
                    str.Append(this.buildLine("", 10, "R", ' '));
                    str.Append(" ");
                    str.Append("A");
                    str.Append("C33");
                    str.Append("D");
                    str.Append(" ");
                    str.Append(this.buildLine("", 8, "R", ' '));
                }
            }

            return str.ToString();
        }
        private string CreateString_CL2(ShipmentPM myShipment, IQueryable<ShipmentPackage> packages)
        {
            StringBuilder str = new StringBuilder(80);

            if (packages.Count() > 0)
            {
                foreach (ShipmentPackage item in packages)
                {
                    str.Append("CL2");
                    str.Append(this.buildLine(item.CommodityNumber, 10, "L", '0'));
                    str.Append("KG ");
                    str.Append(this.buildLine(item.Quantity.ToString(), 10, "L", '0'));
                    str.Append("0000000250");
                    str.Append(this.buildLine("", 3, "R", ' '));
                    str.Append("0000000000");
                    str.Append("0000001200");
                    str.Append("1A001");
                    str.Append(this.buildLine("NLR", 14, "R", ' '));
                    str.Append(this.buildLine("", 2, "R", ' '));
                }
            }

            return str.ToString();
        }
        private string CreateString_Z(ShipmentPM myShipment)
        {
            StringBuilder str = new StringBuilder(80);
            string date = String.Format("{0:yyyyMMdd }", DateTime.Now.Date);
            int tenantNumber = this.myTenant == null ? 0 : this.myTenant.Id;

            str.Append("Z");
            str.Append(this.buildLine("", 2, "R", ' '));
            str.Append(this.buildLine("", 2, "R", ' '));
            str.Append(this.buildLine(tenantNumber.ToString(), 9, "L", ' '));
            str.Append(this.buildLine("", 6, "R", ' '));
            str.Append("E");
            str.Append("XP");
            str.Append(date);
            str.Append("000001");
            str.Append("N");
            str.Append("661661661");
            str.Append(this.buildLine("", 33, "R", ' '));

            return str.ToString();
        }
        #endregion        
    }
}