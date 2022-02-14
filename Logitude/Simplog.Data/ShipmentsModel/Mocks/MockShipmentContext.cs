using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mocks;
using System.Data.Entity;
using Simplog.Server.Infrastructure.Helpers;
using System.Data.Common;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Mocks
{
    public class  MockShipmentContext: IShipmentsContext
    {
        public IDbSet<Shipment> Shipments
        {
            get {

                MockWebFreightContext webcontext = new MockWebFreightContext();
                MockCommonContext commonContext = new MockCommonContext();
                return new MockObjectSet<Shipment>(new List<Shipment>() { 
                  
                new Shipment() {Tenant=1, Id = "1-1", IsAccountingClosed = false , FromPort=Ports.Where(d=>d.Id=="1-1").FirstOrDefault() , FromPortId="1-1" , ToPort=Ports.Where(d=>d.Id=="1-2").FirstOrDefault() , ToPortId="1-2" , TransportMode=webcontext.TransportModes.Where(d=>d.Id=="111").FirstOrDefault() , TransportModeId="111", ShipmentLevel=ShipmentLevels.Where(d=>d.Code=="SL").FirstOrDefault(), ShipmentLevelCode="SL"  , EntityStatus=webcontext.EntityStatus.Where(d=>d.Id=="111").FirstOrDefault() , StatusId="111"}, 
                new Shipment() { Id = Guid.NewGuid().ToString(), IsAccountingClosed = true }, 
                new Shipment() { Id = Guid.NewGuid().ToString(), IsAccountingClosed = false }, }); }
        }

        public List<Shipment> GetShipmentData()
        {
            return Shipments.ToList();
        }

        public IDbSet<Direction> Directions
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<TransportMode> transportModeObjectSet;
        List<TransportMode> transportModes;
        public IDbSet<TransportMode> TransportModes
        {
            get
            {
                if (transportModes == null)
                {
                    transportModes = new List<TransportMode>()
                    {
                        new TransportMode(){ Id="1-1" ,Name="Air"  }};

                    transportModeObjectSet = new MockObjectSet<TransportMode>(transportModes);
                }

                return transportModeObjectSet;
            }
        }

        public IDbSet<Branch> Branches { get { throw new NotImplementedException(); } }
        public IDbSet<ShipmentCommodity> ShipmentCommodities { get { throw new NotImplementedException(); } }

        public IDbSet<Department> Departments
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Incoterm> Incoterms
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<ShipmentType> shipmentTypeObjectSet;
        List<ShipmentType> shipmentTypes;
        public IDbSet<ShipmentType> ShipmentTypes
        {
            get
            {
                if (shipmentTypes == null)
                {
                    shipmentTypes = new List<ShipmentType>()
                    {
                        new ShipmentType(){ Id="1-1"  }};

                    shipmentTypeObjectSet = new MockObjectSet<ShipmentType>(shipmentTypes);
                }

                return shipmentTypeObjectSet;
            }
        }

        public IDbSet<Card> Cards
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<User> Users
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Contact> Contacts
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Address> Addresses
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentMasterData> ShipmentMasterDatas
        {
            get { return new MockObjectSet<ShipmentMasterData>(); }
        }

        MockObjectSet<Port> portObjectSet;
        List<Port> ports;
        public IDbSet<Port> Ports
        {

            get
            {
                if (ports == null)
                {
                    ports = new List<Port>()
                    {
                        new Port(){ Id="1-1" , Tenant=1  },
                    new Port(){ Id="1-2" , Tenant=1  }};

                    portObjectSet = new MockObjectSet<Port>(ports);
                }

                return portObjectSet;
            }
        }

        public IDbSet<FollowUp> FollowUps
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentReceivable> ShipmentReceivables
        {
            get
            {
                return new MockObjectSet<ShipmentReceivable>(new List<ShipmentReceivable>()
                {
                    new ShipmentReceivable()
                    {
                        AmountInProfitCurrency=1000,
                        TotalAmount = 1000,
                        TotalAmountLocal = 1000,
                        Quantity=100,
                        UnitPrice=10,
                        MeasurementId = "1-1",
                        ChargesTypeId = "1-1",
                        ChargesType = new ChargesType() { Id = "1-1", Code = "FRT" },
                        Rate = 1,
                        ProfitCurrencyExchangeRate = 1,
                        Tenant=1,
                        ShipmentId="1-1",
                        Id="1-1",
                        CurrencyId = "1-1",
                        ARInvoiceId = "1-1",
                        PrepaidCollectId = "P",
                        DueTypeCode = "AG",
                        CreatedByUserId = "1-1",
                        ARInvoiceLineId = "1-1",
                       
                    }

                });
            }
        }

        public IDbSet<ChargesType> ChargesTypes
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<Measurement> UnitOfMeasurements
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<ShipmentPickUpDelivery> shipmentPickUpDeliveriesObjectSet;
        List<ShipmentPickUpDelivery> shipmentPickUpDeliveries;
        public IDbSet<ShipmentPickUpDelivery> ShipmentPickUpDeliveries
        {

            get
            {
                if (shipmentPickUpDeliveries == null)
                {
                    shipmentPickUpDeliveries = new List<ShipmentPickUpDelivery>()
                    {
                        new ShipmentPickUpDelivery(){ Id="1-1" , Tenant=1  },
                    new ShipmentPickUpDelivery(){ Id="1-2" , Tenant=1  }};

                    shipmentPickUpDeliveriesObjectSet = new MockObjectSet<ShipmentPickUpDelivery>(shipmentPickUpDeliveries);
                }

                return shipmentPickUpDeliveriesObjectSet;
            }
        }

        public IDbSet<PackageType> PackageTypes
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<ShipmentPackage> shipmentPackageObjectSet;
        List<ShipmentPackage> shipmentPackages;
        public IDbSet<ShipmentPackage> ShipmentPackages
        {
            get
            {
                if (shipmentPackages == null)
                {
                    shipmentPackages = new List<ShipmentPackage>()
                    {
                        new ShipmentPackage(){ Id="1-1" , Tenant=1  },
                    new ShipmentPackage(){ Id="1-2" , Tenant=1  }};

                    shipmentPackageObjectSet = new MockObjectSet<ShipmentPackage>(shipmentPackages);
                }

                return shipmentPackageObjectSet;
            }
        }


        public IDbSet<InsideShipmentPackage> InsideShipmentPackages
        {
            get { throw new NotImplementedException(); }
        }


        MockObjectSet<ShipmentPickUpDeliveryPackage> shipmentPickUpDeliveryPackageObjectSet;
        List<ShipmentPickUpDeliveryPackage> shipmentPickUpDeliveryPackages;
        public IDbSet<ShipmentPickUpDeliveryPackage> ShipmentPickUpDeliveryPackages
        {
            get
            {
                if (shipmentPickUpDeliveryPackages == null)
                {
                    shipmentPickUpDeliveryPackages = new List<ShipmentPickUpDeliveryPackage>()
                    {
                        new ShipmentPickUpDeliveryPackage(){ Id="1-1" , Tenant=1  },
                    new ShipmentPickUpDeliveryPackage(){ Id="1-2" , Tenant=1  }};

                    shipmentPickUpDeliveryPackageObjectSet = new MockObjectSet<ShipmentPickUpDeliveryPackage>(shipmentPickUpDeliveryPackages);
                }

                return shipmentPickUpDeliveryPackageObjectSet;
            }
        }


        MockObjectSet<ShipmentOrderPackage> shipmentOrderPackageObjectSet;
        List<ShipmentOrderPackage> shipmentOrderPackages;
        public IDbSet<ShipmentOrderPackage> ShipmentOrderPackages
        {
            get
            {
                if (shipmentOrderPackages == null)
                {
                    shipmentOrderPackages = new List<ShipmentOrderPackage>()
                    {
                        new ShipmentOrderPackage(){ Id="1-1" , Tenant=1  },
                    new ShipmentOrderPackage(){ Id="1-2" , Tenant=1  }};

                    shipmentOrderPackageObjectSet = new MockObjectSet<ShipmentOrderPackage>(shipmentOrderPackages);
                }

                return shipmentOrderPackageObjectSet;
            }
        }

        public IDbSet<ShipmentPayableLineStatus> ShipmentPayableLineStatus
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ShipmentPayable> ShipmentPayables
        {
            get
            {
                return new MockObjectSet<ShipmentPayable>(new List<ShipmentPayable>()
                {
                    new ShipmentPayable()
                    {
                        AccountedAmount = 0,
                        AccountedAmountInLocalCurrency = 0,
                        AccountedAmountInProfitCurrency = 0,
                        ChargesTypeId = "1-1",
                        ChargesType = new ChargesType() { Id = "1-1", Code = "FRT" },
                        ExpectedAmount = 1000,
                        ExpectedAmountInProfitCurrency = 1000,
                        ExpectedAmountLocal = 1000,
                        OpenAmount = 1000,
                        OpenAmountInLocalCurrency = 1000,
                        OpenAmountInProfitCurrency = 1000,
                        Rate = 1,
                        ProfitCurrencyExchangeRate = 1,
                        Tenant=1,
                        ShipmentId="1-1",
                        Id="1-1",
                    }

                });
            }
        }

        public IDbSet<ShipmentReceivableLineStatus> ShipmentReceivableLineStatus
        {
            get { throw new NotImplementedException(); }
        }

        MockObjectSet<ShipmentReceivableStatus> shipmentReceivableStatusObjectSet;
        List<ShipmentReceivableStatus> shipmentReceivableStatusList;
        public IDbSet<ShipmentReceivableStatus> ShipmentReceivableStatus
        {
            get
            {
                if (shipmentReceivableStatusList == null)
                {
                    shipmentReceivableStatusList = new List<ShipmentReceivableStatus>()
                    {
                        new ShipmentReceivableStatus(){ Code="SRS" }};

                    shipmentReceivableStatusObjectSet = new MockObjectSet<ShipmentReceivableStatus>(shipmentReceivableStatusList);
                }

                return shipmentReceivableStatusObjectSet;
            }
        }

        MockObjectSet<ShipmentPayableStatus> shipmentPayableStatusObjectSet;
        List<ShipmentPayableStatus> shipmentPayableStatus;
        public IDbSet<ShipmentPayableStatus> ShipmentPayableStatus
        {
            get
            {
                if (shipmentPayableStatus == null)
                {
                    shipmentPayableStatus = new List<ShipmentPayableStatus>()
                    {
                        new ShipmentPayableStatus(){ Code="SPS" }};

                    shipmentPayableStatusObjectSet = new MockObjectSet<ShipmentPayableStatus>(shipmentPayableStatus);
                }

                return shipmentPayableStatusObjectSet;
            }
        }

        MockObjectSet<ShipmentCustomerType> shipmentCustomerTypeObjectSet;
        List<ShipmentCustomerType> shipmentCustomerTypes;
        public IDbSet<ShipmentCustomerType> ShipmentCustomerTypes
        {
            get
            {
                if (shipmentCustomerTypes == null)
                {
                    shipmentCustomerTypes = new List<ShipmentCustomerType>()
                    {
                        new ShipmentCustomerType(){ Code="SCT" , Name="Shipment Customer Type" },
                      new ShipmentCustomerType(){ Code="SWT" , Name="Shipment Customer Type" }
                    };

                    shipmentCustomerTypeObjectSet = new MockObjectSet<ShipmentCustomerType>(shipmentCustomerTypes);
                }

                return shipmentCustomerTypeObjectSet;
            }
            
        }

        MockObjectSet<PickUpDeliveryType> pickUpDeliveryTypeObjectSet;
        List<PickUpDeliveryType> pickUpDeliveryTypes;
        public IDbSet<PickUpDeliveryType> PickUpDeliveryTypes
        {
            get
            {
                if (pickUpDeliveryTypes == null)
                {
                    pickUpDeliveryTypes = new List<PickUpDeliveryType>()
                    {
                        new PickUpDeliveryType(){ Code="PUD" }};

                    pickUpDeliveryTypeObjectSet = new MockObjectSet<PickUpDeliveryType>(pickUpDeliveryTypes);
                }

                return pickUpDeliveryTypeObjectSet;
            }
        }

        MockObjectSet<PickUpDeliveryFromToType> pickUpDeliveryFromToTypeObjectSet;
        List<PickUpDeliveryFromToType> pickUpDeliveryFromToTypes;
        public IDbSet<PickUpDeliveryFromToType> PickUpDeliveryFromToTypes
        {
            get
            {
                if (pickUpDeliveryFromToTypes == null)
                {
                    pickUpDeliveryFromToTypes = new List<PickUpDeliveryFromToType>()
                    {
                        new PickUpDeliveryFromToType(){ Code="PUP" }};

                    pickUpDeliveryFromToTypeObjectSet = new MockObjectSet<PickUpDeliveryFromToType>(pickUpDeliveryFromToTypes);
                }

                return pickUpDeliveryFromToTypeObjectSet;
            }
        }

        MockObjectSet<ShipmentAWBPrintOnly> shipmentAWBPrintOnlyObjectSet;
        List<ShipmentAWBPrintOnly> shipmentAWBPrintOnlyList;
        public IDbSet<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies
        {
            get
            {
                if (shipmentAWBPrintOnlyList == null)
                {
                    shipmentAWBPrintOnlyList = new List<ShipmentAWBPrintOnly>()
                    {
                        new ShipmentAWBPrintOnly(){ Id="1-1" , Tenant=1}};

                    shipmentAWBPrintOnlyObjectSet = new MockObjectSet<ShipmentAWBPrintOnly>(shipmentAWBPrintOnlyList);
                }

                return shipmentAWBPrintOnlyObjectSet;
            }
        }

        MockObjectSet<NextLeg> nextLegObjectSet;
        List<NextLeg> nextLegs;
        public IDbSet<NextLeg> NextLegs
        {
            get
            {
                if (nextLegs == null)
                {
                    nextLegs = new List<NextLeg>()
                    {
                        new NextLeg(){ Code="NL" , Name="Next Leg"}};

                    nextLegObjectSet = new MockObjectSet<NextLeg>(nextLegs);
                }

                return nextLegObjectSet;
            }
        }

        MockObjectSet<ShipmentPayableAmountType> shipmentPayableAmountTypeObjectSet;
        List<ShipmentPayableAmountType> shipmentPayableAmountTypes;
        public IDbSet<ShipmentPayableAmountType> ShipmentPayableAmountTypes
        {
            get
            {
                if (shipmentPayableAmountTypes == null)
                {
                    shipmentPayableAmountTypes = new List<ShipmentPayableAmountType>()
                    {
                        new ShipmentPayableAmountType(){ Code="SPAT" }};

                    shipmentPayableAmountTypeObjectSet = new MockObjectSet<ShipmentPayableAmountType>(shipmentPayableAmountTypes);
                }

                return shipmentPayableAmountTypeObjectSet;
            }
        }

        MockObjectSet<ShipmentLevel> shipmentLevelObjectSet;
        List<ShipmentLevel> shipmentLevels;
        public IDbSet<ShipmentLevel> ShipmentLevels
        {
            get
            {
                if (shipmentLevels == null)
                {
                    shipmentLevels = new List<ShipmentLevel>()
                    {
                        new ShipmentLevel(){ Code="SL" , Name="Heigh" }};

                    shipmentLevelObjectSet = new MockObjectSet<ShipmentLevel>(shipmentLevels);
                }

                return shipmentLevelObjectSet;
            }
        }

        public void SetAsModified(object entity)
        {
            
        }

        public void DetectChanges()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 1;
        }

        MockObjectSet<AWBChargesCode> aWBChargeCodeObjectSet;
        List<AWBChargesCode> aWBChargeCodes;
        public IDbSet<AWBChargesCode> AWBChargeCodes
        {
            get {
                if (aWBChargeCodes == null)
                {
                    aWBChargeCodes = new List<AWBChargesCode>()
                    {
                        new AWBChargesCode(){ Code="AWB" , Name="Charge Code"}};

                    aWBChargeCodeObjectSet = new MockObjectSet<AWBChargesCode>(aWBChargeCodes);
                    }

                return aWBChargeCodeObjectSet;
                }
            
            }


        MockObjectSet<AWBSpecialHandlingCode> aWBSpecialHandlingCodeObjectSet;
        List<AWBSpecialHandlingCode> aWBSpecialHandlingCodes;
        public IDbSet<AWBSpecialHandlingCode> AWBHandlingCodes
        {
            get
            {
                if (aWBSpecialHandlingCodes == null)
                {
                    aWBSpecialHandlingCodes = new List<AWBSpecialHandlingCode>()
                    {
                        new AWBSpecialHandlingCode(){ Code="AWB" , Name="Handling Code"}};

                    aWBSpecialHandlingCodeObjectSet = new MockObjectSet<AWBSpecialHandlingCode>(aWBSpecialHandlingCodes);
                }

                return aWBSpecialHandlingCodeObjectSet;
            }
            
            
        }


        public IDbSet<FWBStatus> FWBStatus
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<FHLStatus> FHLStatus
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<AWBStatus> AWBStatus
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ShipmentCarrierStatus> ShipmentCarrierStatuses
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<AWBOCI> AWBOCIs
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<AWBCustomsInformation> AWBCustomsInformations
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<AWBInformation> AWBInformations
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ShipmentPackageItem> ShipmentPackageItems
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<MessagingStock> MessagingStocks
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<MessagingStockUsageHistory> MessagingStockUsageHistories
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<AccountingInformationIdentifier> AccountingInformationIdentifiers
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<SpecialServicesType> SpecialServicesTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ManifestStatus> ManifestStatus
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<AWBAdditionalHandlingInfo> AWBAdditionalHandlingInfos
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public DbConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public DbContext GetActiveDbContext()
        {
            throw new NotImplementedException();
        }

        public IDbSet<ShipmentComputedFields> ShipmentComputedFields
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<ContainersExternalData> ContainersExternalDatas
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<OceanInsightsRequest> OceanInsightsRequests
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<LogitudeOceanInsightsRequest> LogitudeOceanInsightsRequests
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<LogitudeOceanInsightsResponse> LogitudeOceanInsightsResponses
        {
            get { throw new NotImplementedException(); }
        }

        public IDbSet<OceanInsightsRequestsCount> OceanInsightsRequestsCounts
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<OceanInsightsStatuses> OceanInsightsStatuses
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<OtherParticipantId> OtherParticipantIds
        {
            get { throw new NotImplementedException(); }
        }
        public IDbSet<ShipmentAdditionalCloudData> ShipmentAdditionalCloudDatas
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<FBLStock> FBLStocks
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<CustomsTransmissionsStatus> CustomsTransmissionsStatus
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<OBLType> OBLTypes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ShipmentAssembly> ShipmentAssemblies
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ShipmentCustomsMessageType> ShipmentCustomsMessageTypes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IQueryable<TOutput> FunctionTableValue<TOutput>(string functionName, System.Data.SqlClient.SqlParameter[] parameters)
        {
            throw new NotImplementedException();
        }
        public IQueryable<ShipmentDataView> ShipmentSearch(string SearchFields)
        {
            throw new NotImplementedException();
        }
        public IDbSet<ShipmentCustomsTransmission> ShipmentCustomsTransmissions
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<INTTRASIStatus> INTTRASIStatus
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<INTTRABookingTransStatus> INTTRABookingTransStatuses
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<INTTRABookingStatus> INTTRABookingStatuses
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<INTTRAStatus> INTTRAStatuses
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ShipmentContainerStatus> ShipmentContainerStatuses
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<PickUpDeliveryTransportMode> PickUpDeliveryTransportModes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<INTTRADocumentType> INTTRADocumentTypes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<ShipmentPackageHarmonize> ShipmentPackageHarmonizes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<PickUpDeliveryPackageHarmonize> PickUpDeliveryPackageHarmonizes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
        public IDbSet<HarmonizeCode> HarmonizeCodes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<CustomsTransferType> CustomsTransferTypes
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<CustomsTransferLine> CustomsTransferLines
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<CustomsTransferHeader> CustomsTransferHeaders
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IDbSet<ShipmentSubType> ShipmentSubTypes => throw new NotImplementedException();

        public IDbSet<ShipmentStoragePricing> ShipmentStoragePricings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<ShipmentProductItem> ShipmentProductItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<Container> Containers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<ContainerStatus> ContainerStatuses { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<ContainerStatusSource> ContainerStatusSources { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDbSet<ShipmentUnassignedField> ShipmentUnassignedFields { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDbSet<ARInvoice> ARInvoicesForReports { get; }

        public IDbSet<PayableProratedAmount> PayableProratedAmounts => throw new NotImplementedException();
    }
}
