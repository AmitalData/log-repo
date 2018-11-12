import { PartnerSL } from './PartnerSL';
import { PortSL } from './PortSL';
import { HouseSL } from './HouseSL';
import { ShipmentPickUpDeliverySL } from './ShipmentPickUpDeliverySL';
import {ShipmentPackagePM} from '../../Shipment/EntityPMs/ShipmentPackagePM';
export class ManifestSL {
    public ShipmentNumber: string;
    public MasterNumber: string;
    public DirectionId: string;
    public TransportModeId: string;

    public IncotermCode: string;
    public IncotermName: string;
    public IncotermId: string;
    public IncotermAddedManually: boolean;
    public TrailerNumber: string;
    MainCarriageATA: Date;
    Transshipment1ATA: Date;
    Transshipment2ATA: Date;
    Transshipment3ATA: Date;
    MAWBOBLDate: Date;
    public TruckNumber: string;
    public CarrierCode: string;
    public CarrierName: string;
    public CarrierId: string;
    public CarrierAddedManually: boolean;

    public FreightPrepaidCollectId: string;
    public OtherPrepaidCollectId: string;
    public ShipmentLevelCode: string;
    public Shipper: PartnerSL;
    public Consignee: PartnerSL;
    public Notify1: PartnerSL;
    public Notify2: PartnerSL;

    public ValueOfGoods: number;
    public GeneralDescriptionOfGoods: string;
    public HouseNumber: string;


    MasterDate: Date;
    public MainCarriageFromPort: PortSL;
    public MainCarriageToPort: PortSL;

    public Transshipment1FromPort: PortSL;
    public Transshipment1ToPort: PortSL;

    public Transshipment2FromPort: PortSL;
    public Transshipment2ToPort: PortSL;

    public Transshipment3FromPort: PortSL;
    public Transshipment3ToPort: PortSL;

    public EntityId: string;
    public LongMaster: string;


    public GrossWeight: number;
    public ChargeableWeight: number;
    public TEU: number;
    public PackagesQuantity: number;



    public Volume : number;
    public VolumetricWeight : number;
    public NumberOfPackages : number;
    public NumberOfContainers : number;
    public IsDangerous: boolean;
    
    public GrossWeightUnitCode: string;
    public ChargeableWeightUnitCode: string;
    public VolumeUnitCode: string;
    public DimensionsUnitCode: string;
    public ChargeableWeightInKG: number;
    public GrossWeightEdited: boolean;
    public GrossWeightInKG: number;

    public OrderGrossWeight: number;
    public ShipmentTypeId: string;
    public ShipmentTypeName: string;
    public AgentReference1: string;
    public AgentReference2: string;
    public AgentName: string;
    public SharedManifestRef: string;

    public ShipperName: string;
    public ConsigneeReference1: string;
    public ConsigneeReference2: string;
    public ShipperReference1: string;
    public ShipperReference2: string;
    public MainHarmonize: string;



    public MainCarriageAirlinePrefix: string;
    public MainCarriageVesselName: string;
    public MainCarriageVesselId: string;
    public MainCarriageVesselCode: string;
    public  MainCarriageVesselAddedManually :boolean;
    
    public HAWBDate: Date;
    public MainCarriageETD: Date;

    public  MainCarriageATD :Date;
    public MainCarriageETA: Date;




    public MainCarriageMAWBOBL: string;
    public MainCarriageMAWBOBLDate: string;
    public InterlineId: string;
    public InterlineCode: string;
    public InterlineName: string;
    public InterlineAddedManually: boolean;
    public MainCarriageCarrierNumber: string;
 


    public Transshipment1CarrierName: string;
    public Transshipment1CarrierCode: string;
    public Transshipment1CarrierId: string;
    public Transshipment1CarrierAddedManually: boolean;
    public Transshipment1AirlinePrefix: string;
    public Transshipment1ETD: Date;
    public Transshipment1ETA: Date;
    public Transshipment1ATD: Date;
    public Transshipment1MAWBOBL: string;
    public Transshipment1CarrierNumber: string;
    public Transshipment1VesselName: string;
    public Transshipment1VesselId: string;
    public Transshipment1VesselCode: string;
    public Transshipment1VesselAddedManually: boolean;



    public Transshipment2CarrierName: string;
    public Transshipment2CarrierCode: string;
    public Transshipment2CarrierId: string;
    public Transshipment2CarrierAddedManually: boolean;
    public Transshipment2AirlinePrefix: string;
    public Transshipment2ETD: Date;
    public Transshipment2ETA: Date;
    public Transshipment2ATD: Date;
    public Transshipment2MAWBOBL: string;
    public Transshipment2CarrierNumber: string;
    public Transshipment2VesselName: string;
    public Transshipment2VesselId: string;
    public Transshipment2VesselCode: string;
    public Transshipment2VesselAddedManually: boolean;


    public Transshipment3CarrierName: string;
    public Transshipment3CarrierCode: string;
    public Transshipment3CarrierId: string;
    public Transshipment3CarrierAddedManually: boolean;
    public Transshipment3AirlinePrefix: string;
    public Transshipment3ETD: Date;
    public Transshipment3ETA: Date;
    public Transshipment3ATD: Date;
    public Transshipment3MAWBOBL: string;
    public Transshipment3CarrierNumber: string;
    public Transshipment3VesselName: string;
    public Transshipment3VesselId: string;
    public Transshipment3VesselCode: string;
    public Transshipment3VesselAddedManually: boolean;

    public MoveTypeAddedManually: boolean;
    public MoveTypeId: string;
    public MoveTypeName: string;
    public MoveTypeCode: string;
    public MoveTypeTransportModeId: string;

 
    public ValueOfGoodsCurrencyId: string;
    public ValueOfGoodsCurrencyName: string;
    public ValueOfGoodsCurrencyCode: string;
    public ValueOfGoodsCurrencyAddedManually: boolean;


    ShipmentPickUp: ShipmentPickUpDeliverySL;
    ShipmentDelivery: ShipmentPickUpDeliverySL;

    public get Houses(): Array<HouseSL> {
        if (this.houses == null) {
            this.houses = new Array<HouseSL>();
        }
        return this.houses;
    }
    public set Houses(value: Array<HouseSL>) {
        this.houses = value;
    }
    houses: Array<HouseSL>;

    private shipmentPackages: ShipmentPackagePM[];
    get ShipmentPackages() {
        if (this.shipmentPackages == null) {
            this.shipmentPackages = [];
        }

        return this.shipmentPackages;
    }
    set ShipmentPackages(newValue: ShipmentPackagePM[]) {
        if (this.shipmentPackages != newValue) {
            this.shipmentPackages = newValue;
        }
    }

}