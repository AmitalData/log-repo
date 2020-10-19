using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.XSD.INTTRA;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.XSD.INTTRA_Booking
{
    public class INTRABookingContext
    {
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public ICommonDataContext CommonContext;
        private ComputingPartnerTranslationHelper computingPartnerHelper;
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; }
        public INTTRAGeneralMethods iNTTRAGeneralMethods;

        public INTRABookingContext(int teannt, string shipmentId, Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact, ICommonDataContext CommonContext)
        {
            this.Tenant = teannt;
            this.ShipmentId = shipmentId;
            this.LoggedContact = loggedContact;
            this.CommonContext = CommonContext;
            this.computingPartnerHelper = new ComputingPartnerTranslationHelper(Tenant);
            this.iNTTRAGeneralMethods = new INTTRAGeneralMethods();
            this.GetObjects();
            this.GetProperties();
        }

        // Objects & Validating
        public Shipment Shipment { get; set; }
        public ShipmentPM ShipmentPM { get; set; }
        public ShipmentMasterData MasterData { get; set; }
        public string INTTRA_Alias { get; set; }
        public string INTTRA_BookingSettingsId { get; set; }
        public string INTTRA_OutSettingsId { get; set; }
        private Tenant TenantObject;
        private Address TenantAddress;
        public Port FromPort;
        private Port FinalPort;
        private Branch Branch;
        private MoveType MoveType;
        private string MoveType_Name;
        public Contact LoggedContact;
        private Contact BranchContact;
        public Country FromPortCountry;
        private Country FinalPortCountry;
        public ShippingLine MainShippingLine;
        public List<ShipmentOrderPackage> ShipmentOrderPackages = new List<ShipmentOrderPackage>();
        public List<ShipmentPackage> ShipmentPackages = new List<ShipmentPackage>();
        private List<InsideShipmentPackage> InsidePackages = new List<InsideShipmentPackage>();
        private List<ShipmentPackageHarmonize> AllHarmonizes = new List<ShipmentPackageHarmonize>();
        public IShipmentsContext shipmentContext;
        public ShipmentRepository shipmentRepository;
        public ShipmentQuery shipmentQuery;
        public CardQuery cardQuery;
        public ContactQuery contactQuery;

        private ShipmentMasterDataRepository shipmentMasterDataRepository;
        private Vessel MainVessel;
        private void GetObjects()
        {
            this.Errors = new List<string>();
            this.shipmentContext = ShipmentsContext.GetContext(Tenant);
            this.shipmentRepository = new ShipmentRepository(shipmentContext);
            this.cardQuery = new CardQuery(Tenant);
            this.contactQuery = new ContactQuery(Tenant);
            this.shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentContext);
            this.shipmentQuery = new ShipmentQuery(Tenant);
            this.Shipment = shipmentRepository.GetSingleShipment(ShipmentId, Tenant);
            this.ShipmentPM = shipmentQuery.GetSinglePM(ShipmentId, Tenant);
            this.MasterData = shipmentMasterDataRepository.GetSingleMasterData(Shipment.MasterShipmentDataId);
            this.GetObjects_INTTRASetting();
            this.GetObjects_Partners();
            this.GetObjects_Tenant();
            this.GetObjects_MoveType();
            this.GetObjects_ShipmentPorts();
            this.GetObjects_ShipmentCarrier();
            this.GetObjects_Branch();
            this.GetObjects_ShipmentFields();
            this.GetObjects_ShipmentOrderPackages();
            this.IsValid = this.Errors.Count == 0 ? true : false;

        }
        private void GetObjects_Tenant()
        {
            this.TenantObject = (from d in CommonContext.Tenants where d.Id == this.Tenant select d).FirstOrDefault();

            if (this.TenantObject.AddressId == null)
            {
                this.Errors.Add("Tenant Address is required");
            }

            else
            {
                this.TenantAddress = (from d in CommonContext.Addresses where d.Id == this.TenantObject.AddressId select d).FirstOrDefault();

                if (this.TenantAddress != null)
                {
                    if (string.IsNullOrEmpty(this.TenantAddress.Address1) && string.IsNullOrEmpty(this.TenantAddress.Address2))
                    {
                        this.Errors.Add("Tenant Address 1 or Address 2 is required");
                    }
                }
            }
        }
        private void GetObjects_MoveType()
        {
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(this.Tenant);

            this.MoveType = (from d in webFreightContext.MoveTypes where d.Id == this.Shipment.MoveTypeId select d).FirstOrDefault();
            if (this.MoveType != null)
            {
                this.MoveType_Name = this.MoveType.MoveTypeEnglishName;

                string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(this.MoveType.Code, "G-INTTRA", "MoveType");
                if (!string.IsNullOrEmpty(myTranslatedCode))
                {
                    this.MoveType_Name = myTranslatedCode;
                }

                switch (this.MoveType_Name.ToLower())
                {
                    case "doortodoor":
                        {
                            if (this.Shipment.PreCarriageFromPortId == null || this.Shipment.PreCarriageToPortId == null)
                            {
                                this.Errors.Add("Pre Carriage is required");
                            }

                            if (this.Shipment.OnCarriageFromPortId == null || this.Shipment.OnCarriageToPortId == null)
                            {
                                this.Errors.Add("On Carriage is required");
                            }

                            break;
                        }

                    case "doortoport":
                        {
                            if (this.Shipment.PreCarriageFromPortId == null || this.Shipment.PreCarriageToPortId == null)
                            {
                                this.Errors.Add("Pre Carriage is required");
                            }

                            break;
                        }

                    case "porttodoor":
                        {
                            if (this.Shipment.OnCarriageFromPortId == null || this.Shipment.OnCarriageToPortId == null)
                            {
                                this.Errors.Add("On Carriage is required");
                            }

                            break;
                        }

                    case "porttoport":
                        {
                            break;
                        }

                    default:
                        {
                            this.Errors.Add("Illegal value in move type");
                            break;
                        }
                }
            }
        }
        private void GetObjects_ShipmentFields()
        {
            if (this.Shipment.INTTRAContractNumber == null)
            {
                this.Errors.Add("Contract Number is required");
            }

            if (this.Shipment.ShipperId == null)
            {
                this.Errors.Add("Shipper is required");
            }

            if (this.Shipment.ConsigneeId == null)
            {
                this.Errors.Add("Consignee is required");
            }

            if (this.Shipment.ConsigneeId == null)
            {
                this.Errors.Add("Consignee is required");
            }

            if (this.MasterData.MainCarriageETD == null)
            {
                if (this.MasterData.MainCarriageVesselId == null || this.MasterData.MainCarriageCarrierNumber == null)
                    this.Errors.Add("ETD or Main-Carriage Vessel and Voyage must be provided");
            }

            if (this.MasterData.MainCarriageVesselId != null)
            {
                this.MainVessel = (from d in CommonContext.Vessels where d.Id == this.MasterData.MainCarriageVesselId select d).FirstOrDefault();
            }
        }
        private void GetObjects_ShipmentOrderPackages()
        {
            if (string.IsNullOrEmpty(this.ShipmentPM.DescriptionOfGoods))
            {
                this.Errors.Add("Shipment Description  of Goods is required");
            }

            this.ShipmentOrderPackages = (from d in shipmentContext.ShipmentOrderPackages
                                          where d.Tenant == this.Tenant
                                          && d.ShipmentId == this.ShipmentId
                                          select d).ToList();
            this.ShipmentPackages = (from d in shipmentContext.ShipmentPackages
                                     where d.Tenant == this.Tenant
                                     && d.ShipmentId == this.ShipmentId
                                     select d).ToList();

            if ((this.ShipmentPackages == null || (this.ShipmentPackages != null && this.ShipmentPackages.Count() == 0))
                &&
                (this.ShipmentOrderPackages == null || (this.ShipmentOrderPackages != null && this.ShipmentOrderPackages.Count() == 0)))
            {
                this.Errors.Add("Shipment Order Packages or Shipment Packages are required");
            }
        }

        private Address ShipperAddress;
        private Address ConsigneeAddress;
        private void GetObjects_Partners()
        {
            if (!string.IsNullOrEmpty(this.Shipment.ShipperAddressId))
            {
                this.ShipperAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.ShipperAddressId select d).FirstOrDefault();

                if (this.ShipperAddress != null)
                {
                    if (string.IsNullOrEmpty(this.ShipperAddress.Address1) && string.IsNullOrEmpty(this.ShipperAddress.Address2))
                    {
                        this.Errors.Add("Shipper Address 1 or Address 2 is required");
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.Shipment.ConsigneeAddressId))
            {
                this.ConsigneeAddress = (from d in CommonContext.Addresses where d.Id == this.Shipment.ConsigneeAddressId select d).FirstOrDefault();

                if (this.ConsigneeAddress != null)
                {
                    if (string.IsNullOrEmpty(this.ConsigneeAddress.Address1) && string.IsNullOrEmpty(this.ConsigneeAddress.Address2))
                    {
                        this.Errors.Add("Consignee Address 1 or Address 2 is required");
                    }
                }
            }
        }

        private void GetObjects_ShipmentPorts()
        {
            if (this.MasterData.MainCarriageFromPortId == null)
            {
                this.Errors.Add("From Port is required");
            }

            else
            {
                this.FromPort = (from d in CommonContext.Ports where d.Id == this.MasterData.MainCarriageFromPortId select d).FirstOrDefault();
                this.FromPortCountry = (from d in CommonContext.Countries where d.Id == this.FromPort.CountryId select d).FirstOrDefault();
            }

            if (this.MasterData.MainCarriageFinalDestinationPortId == null)
            {
                this.Errors.Add("Final Destination Port is required");
            }

            else
            {
                this.FinalPort = (from d in CommonContext.Ports where d.Id == this.MasterData.MainCarriageFinalDestinationPortId select d).FirstOrDefault();
                this.FinalPortCountry = (from d in CommonContext.Countries where d.Id == this.FinalPort.CountryId select d).FirstOrDefault();
            }
        }
 
        private void GetObjects_ShipmentCarrier()
        {
            if (this.MasterData.MainCarriageCarrierId == null)
            {
                this.Errors.Add("Main Carriage Carrier is required");
            }
            else
            {
                this.MainShippingLine = (from d in CommonContext.ShippingLines where d.Id == this.MasterData.MainCarriageCarrierId select d).FirstOrDefault();
            }
        }
        private void GetObjects_Branch()
        {
            if (this.Shipment.BranchId == null)
            {
                this.Errors.Add("Branch is required");
            }

            else
            {
                this.Branch = (from d in CommonContext.Branches where d.Id == this.Shipment.BranchId select d).FirstOrDefault();

                if (this.Branch.INTTRAId == null)
                {
                    this.Errors.Add("Branch INTTRA ID is required");
                }

                if (this.Branch.INTTRAAlias == null)
                {
                    this.Errors.Add("Branch INTTRA Alias is required");
                }

                if (this.Branch.INTTRAContactId == null)
                {
                    this.Errors.Add("Branch INTTRA Contact is required");
                }

                else
                {
                    this.BranchContact = (from d in CommonContext.Contacts where d.Id == this.Branch.INTTRAContactId select d).FirstOrDefault();

                    if (this.BranchContact.Email == null)
                    {
                        this.Errors.Add("Branch INTTRA Contact Email is required");
                    }
                }
            }
        }

        public string CommunicationLogIdCounter { get; set; }
        public void Build()
        {
            this.CommunicationLogIdCounter = IdCounter.GetNumber("CommunicationLog", Tenant);
            this.BuildMessageHeader();
            this.BuildMessageProperties();
            this.BuildMessageDetails();
        }

        // Message Header
        public INTTRA_Booking.HeaderType Sender;
        public INTTRA_Booking.HeaderType Recipient;
        private void BuildMessageHeader()
        {
            this.BuildMessageHeader_Sender();
            this.BuildMessageHeader_Recipient();


        }
        private void BuildMessageHeader_Sender()
        {
           
        }
        private void BuildMessageHeader_Recipient()
        {
            
        }

        // Message Properties
        public INTTRA_Booking.MovementTypeType MovementType;
        public List<INTTRA_Booking.ReferenceInformationType> ReferenceInformations;
        public List<INTTRA_Booking.LocationDateTimeType> Locations;
        public List<INTTRA_Booking.TransportationDetailsType> TransportationDetails;
        public List<INTTRA_Booking.PartiesType> MessagePropertiesParties;

        private void BuildMessageProperties()
        {
            this.BuildMessageProperties_MovementType();
            this.BuildMessageProperties_ReferenceInformations();
            this.BuildMessageProperties_Locations();
            this.BuildMessageProperties_TransportationDetails();
            this.BuildMessageProperties_Parties();
        }

        private void BuildMessageProperties_MovementType()
        {
            if (this.MoveType != null)
            {
                switch (this.MoveType_Name.ToLower())
                {
                    case "doortodoor":
                        {
                            this.MovementType = INTTRA_Booking.MovementTypeType.DoorToDoor;
                            break;
                        }

                    case "doortoport":
                        {
                            this.MovementType = INTTRA_Booking.MovementTypeType.DoorToPort;
                            break;
                        }

                    case "porttoport":
                        {
                            this.MovementType = INTTRA_Booking.MovementTypeType.PortToPort;
                            break;
                        }

                    case "porttodoor":
                        {
                            this.MovementType = INTTRA_Booking.MovementTypeType.PortToDoor;
                            break;
                        }
                }
            }
        }
        private void BuildMessageProperties_ReferenceInformations()
        {
            this.ReferenceInformations = new List<INTTRA_Booking.ReferenceInformationType>();
            if (this.MasterData.BookingConfirmationNumber != null)
            {
                this.ReferenceInformations.Add(new INTTRA_Booking.ReferenceInformationType()
                {
                    Type = INTTRA_Booking.ReferenceTypeValues.BookingNumber,
                    Value = this.MasterData.BookingConfirmationNumber,
                });
            }
            if (this.Shipment.INTTRAContractNumber != null)
            {
                this.ReferenceInformations.Add(new INTTRA_Booking.ReferenceInformationType()
                {
                    Type = INTTRA_Booking.ReferenceTypeValues.ContractNumber,
                    Value = this.Shipment.INTTRAContractNumber,
                });
            }
        }
        private void BuildMessageProperties_Locations()
        {
            this.Locations = new List<INTTRA_Booking.LocationDateTimeType>();

            INTTRA_Booking.LocationDateTimeType item_PlaceOfReceipt = new INTTRA_Booking.LocationDateTimeType()
            {
                Type = INTTRA_Booking.LocationTypeValues.PlaceOfReceipt,
                Identifier = new LocationIdentifierType()
                {
                    Type = LocationIdentifierTypeValues.UNLOC,
                    Value = this.FromPort.CombinedCode,
                },
                Name = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),
                CountryCode = this.FromPortCountry.Code.ToUpper(),
            };

            if (this.ShipmentPM.MainCarriageETD != null)
            {
                item_PlaceOfReceipt.DateTime = new LocationDateType()
                {
                    Type = DateTimeCodeTypeType.Date,
                    DateType = LocationDateTypeValues.EarliestDeparture,
                    Value = this.ShipmentPM.MainCarriageETD.Value,
                };
            }

            this.Locations.Add(item_PlaceOfReceipt);

            INTTRA_Booking.LocationDateTimeType item_PlaceOfDelivery = new INTTRA_Booking.LocationDateTimeType()
            {
                Type = INTTRA_Booking.LocationTypeValues.PlaceOfDelivery,
                Identifier = new LocationIdentifierType()
                {
                    Type = LocationIdentifierTypeValues.UNLOC,
                    Value = this.FinalPort.CombinedCode,
                },
                Name = this.iNTTRAGeneralMethods.FormatString(this.FinalPort.EnglishName, 256),
                CountryCode = this.FinalPortCountry.Code.ToUpper(),
            };
            this.Locations.Add(item_PlaceOfDelivery);
        }
        private void BuildMessageProperties_TransportationDetails()
        {
            this.TransportationDetails = new List<TransportationDetailsType>();
            var transportationDetails = new INTTRA_Booking.TransportationDetailsType()
            {
                TransportStage = INTTRA_Booking.TransportationDetailsTypeTransportStage.Main,
                TransportMode = INTTRA_Booking.TransportModeTypeValues.MaritimeTransport,
                
                ConveyanceInformation = new INTTRA_Booking.ConveyanceInformationType()
                {
                    Type = ConveyanceTypeValues.ContainerShip,
                },
            };

            if (this.MasterData.MainCarriageCarrierNumber != null && this.MasterData.MainCarriageVesselId != null)
            {
                transportationDetails.ConveyanceInformation.Identifier = new ConveyanceIdentifierType[]
                {
                    new ConveyanceIdentifierType()
                    {
                        Type = ConveyanceIdentifierTypeValues.VesselName,
                        Value = this.iNTTRAGeneralMethods.FormatString(this.MainVessel.EnglishName, 35),
                    },
                    new ConveyanceIdentifierType()
                    {
                        Type = ConveyanceIdentifierTypeValues.VoyageNumber,
                        Value = this.iNTTRAGeneralMethods.FormatString(this.MasterData.MainCarriageCarrierNumber, 35),
                    },
                };
            }

            if (this.MainShippingLine != null)
            {
                if (this.MainShippingLine.SCACCode != null)
                {
                    transportationDetails.ConveyanceInformation.OperatorIdentifier = new ConveyanceOperatorIDType()
                    {
                        Type = ConveyanceOperatorIDTypeValues.SCACCode,
                        Value = this.iNTTRAGeneralMethods.FormatString(this.MainShippingLine.SCACCode, 35),
                    };
                }
            }

            List<INTTRA_Booking.LocationDateTimeType> locations = new List<INTTRA_Booking.LocationDateTimeType>();

            // From
            var location = new INTTRA_Booking.LocationDateTimeType()
            {
                Type = INTTRA_Booking.LocationTypeValues.PortOfLoad,
                Identifier = new LocationIdentifierType()
                {
                    Type = LocationIdentifierTypeValues.UNLOC,
                    Value = this.FromPort.CombinedCode,
                },
                Name = this.iNTTRAGeneralMethods.FormatString(this.FromPort.EnglishName, 256),
                CountryCode = this.FromPortCountry.Code.ToUpper(),
            };
            
            if (this.ShipmentPM.MainCarriageETD != null)
            {
                location.DateTime = new LocationDateType()
                {
                    Type = DateTimeCodeTypeType.Date,
                    DateType = LocationDateTypeValues.EarliestDeparture,
                    Value = this.ShipmentPM.MainCarriageETD.Value,
                };
            }
            locations.Add(location);

            // Final
            locations.Add(new INTTRA_Booking.LocationDateTimeType()
            {
                Type = INTTRA_Booking.LocationTypeValues.PortOfDischarge,
                Identifier = new LocationIdentifierType()
                {
                    Type = LocationIdentifierTypeValues.UNLOC,
                    Value = this.FinalPort.CombinedCode,
                },
                Name = this.iNTTRAGeneralMethods.FormatString(this.FinalPort.EnglishName, 256),
                CountryCode = this.FinalPortCountry.Code.ToUpper(),           
            });

            transportationDetails.Location = locations.ToArray<INTTRA_Booking.LocationDateTimeType>();
            this.TransportationDetails.Add(transportationDetails);
        }
        private void BuildMessageProperties_Parties()
        {
            this.MessagePropertiesParties = new List<INTTRA_Booking.PartiesType>();

            #region Booker
            if (this.TenantObject != null)
            {
                INTTRA_Booking.PartiesType item = new INTTRA_Booking.PartiesType()
                {
                    Role = INTTRA_Booking.PartyTypeValues.Booker,
                    RoleSpecified  =true,
                    Name = this.iNTTRAGeneralMethods.GetStringList(this.TenantObject.Company, 2, 35).FirstOrDefault(),
                    Identifier = new INTTRA_Booking.PartyIdentifierType()
                    {
                        Type  = PartyIdentifierTypeValues.PartnerAlias,
                        Value = this.Branch.INTTRAAlias,
                    },
                };

                if (this.TenantAddress != null)
                {
                    item.Address = this.GetAddressInformation(this.TenantAddress);
                }
                this.MessagePropertiesParties.Add(item);
            }
            #endregion
            #region Forwarder
            if (this.TenantObject != null)
            {
                INTTRA_Booking.PartiesType item = new INTTRA_Booking.PartiesType()
                {
                    Role = INTTRA_Booking.PartyTypeValues.Forwarder,
                    RoleSpecified = true,
                    Name = this.iNTTRAGeneralMethods.GetStringList(this.TenantObject.Company, 2, 35).FirstOrDefault(),
                    Identifier = new INTTRA_Booking.PartyIdentifierType()
                    {
                        Type = PartyIdentifierTypeValues.PartnerAlias,
                        Value = this.Branch.INTTRAAlias,
                    },
                };

                if (this.TenantAddress != null)
                {
                    item.Address = this.GetAddressInformation(this.TenantAddress);
                }
                this.MessagePropertiesParties.Add(item);
            }
            #endregion
            #region Carrier
            if (this.MasterData.MainCarriageCarrierId != null)
            {
                CardPM myCard = cardQuery.GetSinglePM(this.MasterData.MainCarriageCarrierId, Tenant);
                if (myCard != null)
                {
                    INTTRA_Booking.PartiesType item = new INTTRA_Booking.PartiesType()
                    {
                        Role = INTTRA_Booking.PartyTypeValues.Carrier,
                        RoleSpecified = true,
                        Name = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).FirstOrDefault(),
                        Identifier = new INTTRA_Booking.PartyIdentifierType()
                        {
                            Type = PartyIdentifierTypeValues.PartnerAlias,
                            Value = this.Branch.INTTRAAlias,
                        },
                    };
                    if (this.MainShippingLine != null)
                    {
                        if (this.MainShippingLine.SCACCode != null)
                        {
                            item.Identifier = new INTTRA_Booking.PartyIdentifierType()
                            {
                                Type = PartyIdentifierTypeValues.PartnerAlias,
                                Value = this.MainShippingLine.SCACCode,
                            };
                        }
                    }

                    var contacts = new List<INTTRA_Booking.ContactInformationType>();
                    var contactPM = contactQuery.GetSinglePM(myCard.ContactId, Tenant);
                    if (contactPM != null)
                    {
                        contacts.Add(new ContactInformationType()
                        {
                            Type = ContactTypeValues.InformationContact,
                            Name = this.iNTTRAGeneralMethods.GetStringList(contactPM.EnglishName, 2, 35).FirstOrDefault(),
                            CommunicationDetails = new CoordinatesType()
                            {
                                Email = this.iNTTRAGeneralMethods.GetStringList(contactPM.Email, 9, 512).ToArray(),
                                Fax = this.iNTTRAGeneralMethods.GetStringList(contactPM.Fax, 9, 512).ToArray(),
                                Phone = this.iNTTRAGeneralMethods.GetStringList(contactPM.BusinessPhone, 9, 512).ToArray(),
                            }
                        });
                    }

                    item.Contacts = contacts.ToArray();
                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion
            #region Shipper
            if (this.Shipment.ShipperId != null)
            {
                CardPM shipper = cardQuery.GetSinglePM(this.Shipment.ShipperId, Tenant);
                if (shipper != null)
                {
                    INTTRA_Booking.PartiesType item = new INTTRA_Booking.PartiesType()
                    {
                        Role = INTTRA_Booking.PartyTypeValues.Shipper,
                        RoleSpecified = true,
                        Name = this.iNTTRAGeneralMethods.GetStringList(shipper.EnglishName, 2, 35).FirstOrDefault(),
                    };

                    if (this.ShipperAddress != null)
                    {
                        item.Address = this.GetAddressInformation(this.ShipperAddress);
                    }

                    var contacts = new List<INTTRA_Booking.ContactInformationType>();
                    var contactPM = contactQuery.GetSinglePM(this.Shipment.ShipperContactId, Tenant);
                    if (contactPM != null)
                    {
                        if (contactPM.Email == null && contactPM.Fax == null && contactPM.BusinessPhone == null)
                        {
                            this.Errors.Add("At least one of Shipper Information Contact Phone Number, Email Address or Fax Number must be provided.");
                        }

                        contacts.Add(new ContactInformationType()
                        {
                            Type = ContactTypeValues.InformationContact,
                            Name = this.iNTTRAGeneralMethods.GetStringList(contactPM.EnglishName, 2, 35).FirstOrDefault(),
                            CommunicationDetails = new CoordinatesType()
                            {
                                Email = this.iNTTRAGeneralMethods.GetStringList(contactPM.Email, 9, 512).ToArray(),
                                Fax = this.iNTTRAGeneralMethods.GetStringList(contactPM.Fax, 9, 512).ToArray(),
                                Phone = this.iNTTRAGeneralMethods.GetStringList(contactPM.BusinessPhone, 9, 512).ToArray(),
                            }
                        });
                    }

                    else
                    {
                        this.Errors.Add("The Shipper Contact is required.");
                    }
                    item.Contacts = contacts.ToArray();
                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion
            #region Consignee
            if (this.Shipment.ConsigneeId != null)
            {
                CardPM myCard = cardQuery.GetSinglePM(this.Shipment.ConsigneeId, Tenant);
                if (myCard != null)
                {
                    INTTRA_Booking.PartiesType item = new INTTRA_Booking.PartiesType()
                    {
                        Role = INTTRA_Booking.PartyTypeValues.Consignee,
                        RoleSpecified = true,
                        Name = this.iNTTRAGeneralMethods.GetStringList(myCard.EnglishName, 2, 35).FirstOrDefault(),
                    };

                    if (this.ConsigneeAddress != null)
                    {
                        item.Address = this.GetAddressInformation(this.ConsigneeAddress);
                    }

                    var contacts = new List<INTTRA_Booking.ContactInformationType>();
                    var contactPM = contactQuery.GetSinglePM(this.Shipment.ConsigneeContactId, Tenant);
                    if (contactPM != null)
                    {
                        contacts.Add(new ContactInformationType()
                        {
                            Type = ContactTypeValues.InformationContact,
                            Name = this.iNTTRAGeneralMethods.GetStringList(contactPM.EnglishName,2, 35).FirstOrDefault(),
                            CommunicationDetails = new CoordinatesType()
                            {
                                Email = this.iNTTRAGeneralMethods.GetStringList(contactPM.Email, 9, 512).ToArray(),
                                Fax = this.iNTTRAGeneralMethods.GetStringList(contactPM.Fax, 9, 512).ToArray(),
                                Phone = this.iNTTRAGeneralMethods.GetStringList(contactPM.BusinessPhone, 9, 512).ToArray(),
                            }
                        });
                    }
   
                    item.Contacts = contacts.ToArray();
                    this.MessagePropertiesParties.Add(item);
                }
            }
            #endregion
        }

        private INTTRA_Booking.PartyAddressType GetAddressInformation(Address myAddress)
        {
            INTTRA_Booking.PartyAddressType myResult = null;

            if (myAddress != null)
            {
                string iCountryName = null;

                myResult = new INTTRA_Booking.PartyAddressType()
                {
                    CityName = this.iNTTRAGeneralMethods.FormatString(myAddress.City, 35),
                };

                if (!string.IsNullOrEmpty(myAddress.Address2))
                {
                    myResult.StreetAddress = myAddress.Address2;
                }

                if (!string.IsNullOrEmpty(myAddress.ZipCode))
                {
                    myResult.PostalCode = this.iNTTRAGeneralMethods.FormatString(myAddress.ZipCode, 19);
                }

                if (myAddress.CountryId != null)
                {
                    Country myCountry = (from d in CommonContext.Countries where d.Id == myAddress.CountryId select d).FirstOrDefault();

                    if (myCountry != null)
                    {

                        myResult.CountryCode = myCountry.Code;
                        iCountryName = myCountry.EnglishName;

                    }
                }
            }
            return myResult;
        }

        // Message Details
        private int lineNumber_Goods = 1;
        public List<INTTRA_Booking.GoodsDetailsType> GoodsDetails;
        public List<INTTRA_Booking.EquipmentDetailsType> EquipmentDetails;
        private List<PackageType> AllPackageTypes;
        private void BuildMessageDetails()
        {
            this.GoodsDetails = new List<INTTRA_Booking.GoodsDetailsType>();
            this.EquipmentDetails = new List<INTTRA_Booking.EquipmentDetailsType>();

            this.BuildMessageDetails_EquipmentDetails();
            this.BuildMessageDetails_GoodsDetails();
        }
        private void BuildMessageDetails_EquipmentDetails()
        {
            this.AllPackageTypes = (from d in CommonContext.PackageTypes
                                    where d.Tenant == this.Tenant
                                    select d).ToList();

            if (ShipmentPackages.Count > 0)
            {
                BuildMessageDetails_EquipmentDetails_ShipmentPackages();
            }

            else
            {
                BuildMessageDetails_EquipmentDetails_ShipmentOrderPackages();
            }
        }

        private void BuildMessageDetails_EquipmentDetails_ShipmentPackages()
        {
            foreach (var item in ShipmentPackages)
            {
                PackageType packageType = this.AllPackageTypes.Where(a => a.Id == item.PackageTypeId).FirstOrDefault();

                string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(packageType.Code, "G-INTTRA", "PackageType");
                if (string.IsNullOrEmpty(myTranslatedCode))
                {
                    this.Errors.Add("Partner code is required for all Package Types");
                }

                else
                {
                    INTTRA_Booking.EquipmentDetailsType itemDetails = new INTTRA_Booking.EquipmentDetailsType()
                    {
                        EquipmentType = new EquipmentTypeType()
                        {
                            EquipmentTypeCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(packageType.Code, "G-INTTRA", "PackageType"),
                        },

                        NumberOfEquipment = item.Quantity + "",

                        ImportExportHaulage = new ImportExportHaulageType()
                        {
                            HaulageArrangements = ImportExportHaulageTypeHaulageArrangements.MerchantExportHaulageMerchantImportHaulage,
                            CargoMovementType = ImportExportHaulageTypeCargoMovementType.FCLFCL,
                            CargoMovementTypeSpecified = true,
                        }
                    };

                    if (item.NonActiveContainer)
                    {
                        itemDetails.EquipmentTemperature = new INTTRA_Booking.TemperatureType()
                        {
                            UOM = INTTRA_Booking.TemperatureUOMValues.CEL,
                            Value = "999",
                        };

                        if (item.TemperatureUnitCode != "CEL")
                        {
                            itemDetails.EquipmentTemperature.UOM = INTTRA_Booking.TemperatureUOMValues.FAH;
                        }
                    }

                    else if (item.Temperature != null)
                    {
                        item.Temperature = item.Temperature.Trim();

                        if (!string.IsNullOrEmpty(item.Temperature))
                        {
                            if (!this.iNTTRAGeneralMethods.IsDecimalFormat(item.Temperature))
                            {
                                string msg = TranslateTextsClass.Translate("ShipmentPackage.F.Temperature", this.Tenant) + " invalid format";
                                this.Errors.Add(msg);
                            }

                            else
                            {
                                itemDetails.EquipmentTemperature = new INTTRA_Booking.TemperatureType()
                                {
                                    UOM = INTTRA_Booking.TemperatureUOMValues.CEL,
                                    Value = item.Temperature,
                                };

                                if (item.TemperatureUnitCode != "CEL")
                                {
                                    itemDetails.EquipmentTemperature.UOM = INTTRA_Booking.TemperatureUOMValues.FAH;
                                }
                            }
                        }
                    }

                    this.EquipmentDetails.Add(itemDetails);
                }
            }

        }
        private void BuildMessageDetails_EquipmentDetails_ShipmentOrderPackages()
        {
            var groupedPackages = (from d in this.ShipmentOrderPackages
                                   group d by new { d.PackageTypeId } into g
                                   select new
                                   {
                                       PackageTypeId = g.Key.PackageTypeId,
                                       Quantity = g.Sum(s => s.Quantity),
                                   });

            foreach (var item in groupedPackages)
            {
                PackageType packageType = this.AllPackageTypes.Where(a => a.Id == item.PackageTypeId).FirstOrDefault();

                string myTranslatedCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(packageType.Code, "G-INTTRA", "PackageType");
                if (string.IsNullOrEmpty(myTranslatedCode))
                {
                    this.Errors.Add("Partner code is required for all Package Types");
                }

                else
                {
                    INTTRA_Booking.EquipmentDetailsType itemDetails = new INTTRA_Booking.EquipmentDetailsType()
                    {
                        EquipmentType = new EquipmentTypeType()
                        {
                            EquipmentTypeCode = computingPartnerHelper.GetComputingPartnerCodeTranslation(packageType.Code, "G-INTTRA", "PackageType"),
                        },

                        NumberOfEquipment = item.Quantity + "",
                        ImportExportHaulage = new ImportExportHaulageType()
                        {
                            HaulageArrangements = ImportExportHaulageTypeHaulageArrangements.MerchantExportHaulageMerchantImportHaulage,
                            CargoMovementType = ImportExportHaulageTypeCargoMovementType.FCLFCL,
                            CargoMovementTypeSpecified = true,
                        }
                    };

                    this.EquipmentDetails.Add(itemDetails);
                }
            }
        }

        private void BuildMessageDetails_GoodsDetails()
        {
            string descriptionOfGoods = FormatHelper.FormatString(this.ShipmentPM.DescriptionOfGoods, FormatHelper.PatternType.NatureAndQuantityOfGoods);

            INTTRA_Booking.GoodsDetailsType itemDetails = new INTTRA_Booking.GoodsDetailsType()
            {
                LineNumber = "1",
                GoodDescription = !string.IsNullOrEmpty(descriptionOfGoods) ? this.iNTTRAGeneralMethods.GetStringList(descriptionOfGoods, 2, 1024).FirstOrDefault(): null,
                PackageDetail = new PackageDetailType()
                {
                    OuterPack = new OuterPackType(),
                    
                },
                DetailsReferenceInformation = new ReferenceInformationType[]
                {
                    new ReferenceInformationType()
                    {
                        Type = ReferenceTypeValues.FreightForwarderRefNumber,
                        Value = this.Shipment.ShipmentNumber,
                    }
                },
            };

            this.GoodsDetails.Add(itemDetails);
        }

        public System.DateTime TodayDate { get; set; }
        public System.DateTime TodayDateTime { get; set; }
        public string ShipmentNumber { get; set; }
        public string VolumeUnitCode { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public long XMLCreateDate { get; set; }
        private void GetProperties()
        {
            this.TodayDate = TenantServerConfigration.GetCurrentDateTime(this.Tenant).Date;
            this.TodayDateTime = TenantServerConfigration.GetCurrentDateTime(this.Tenant);
            this.XMLCreateDate = this.iNTTRAGeneralMethods.GetDateShortFormat(this.TodayDateTime);

            this.ShipmentNumber = this.Shipment.ShipmentNumber;
            this.VolumeUnitCode = this.Shipment.VolumeUnitCode.ToUpper();
            this.GrossWeightUnitCode = this.Shipment.GrossWeightUnitCode.ToUpper();
        }

        private void GetObjects_INTTRASetting()
        {
            INTTRASetting iSetting = (from d in CommonContext.INTTRASettings where d.Tenant == this.Tenant select d).FirstOrDefault();
            if (iSetting != null)
            {
                this.INTTRA_Alias = iSetting.INTTRAAlias;
                this.INTTRA_OutSettingsId = iSetting.OutSettingsId;
            }

            if (string.IsNullOrEmpty(this.INTTRA_Alias) || string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
            {
                iSetting = (from d in CommonContext.INTTRASettings where d.Tenant == 0 select d).FirstOrDefault();

                if (iSetting != null)
                {
                    if (string.IsNullOrEmpty(this.INTTRA_Alias))
                    {
                        this.INTTRA_Alias = iSetting.INTTRAAlias;
                    }

                    if (string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
                    {
                        this.INTTRA_OutSettingsId = iSetting.OutSettingsId;
                    }
                }
            }

            if (string.IsNullOrEmpty(this.INTTRA_Alias))
            {
                this.Errors.Add("INTTRA Alias is required");
            }

            if (string.IsNullOrEmpty(this.INTTRA_OutSettingsId))
            {
                this.Errors.Add("Out Settings is required");
            }
        }
    }
}
