import { PartnerSL } from './PartnerSL';
import {ShipmentPackagePM} from '../../Shipment/EntityPMs/ShipmentPackagePM';
export class HouseSL {
    public HouseNumber: string;
    public ShipmentNumber: string;
    public Shipper: PartnerSL;
    public FreightPrepaidCollectId: string;
    public OtherPrepaidCollectId: string;
    public SharedManifestRef: string;
    public IsDangerous: boolean;

    public IncotermCode: string;
    public IncotermName: string;
    public IncotermId: string;
    public IncotermAddedManually: boolean;
    public ShipperName: string;
    public HAWBDate: Date;
    public CarrierCode: string;
    public CarrierName: string;
    public CarrierId: string;
    public CarrierAddedManually: boolean;
    public MainCarriageCarrierNumber: string;
    public ValueOfGoods: number;
    
    public ConsigneeReference1: string;
    public ConsigneeReference2: string;
    public ShipperReference1: string;
    public ShipperReference2: string;
    public MainHarmonize: string;


    public EntityId: string;
    public Consignee: PartnerSL;
    public Notify1: PartnerSL;
    public Notify2: PartnerSL;

    public GeneralDescriptionOfGoods: string;

    public GrossWeight: number;
    public ChargeableWeight: number;
    public TEU: number;
    public PackagesQuantity: number;

    public Volume: number;
    public VolumetricWeight: number;
    public NumberOfPackages: number;
    public NumberOfContainers: number;
    
    public AgentName: string;

    public  GrossWeightUnitCode : string;
    public  ChargeableWeightUnitCode: string;
    public  VolumeUnitCode : string;
    public  DimensionsUnitCode: string;
    public ChargeableWeightInKG: number;
    public GrossWeightEdited: boolean;
    public  GrossWeightInKG: number;

    public OrderGrossWeight: number;
    public ShipmentTypeId: string;
    public ShipmentTypeName: string;

    public MoveTypeId: string;
    public MoveTypeName: string;
    public  MoveTypeCode: string;
    public MoveTypeAddedManually: boolean;
    public MoveTypeTransportModeId: string;

    public ValueOfGoodsCurrencyId: string;
    public ValueOfGoodsCurrencyName: string;
    public ValueOfGoodsCurrencyCode: string;
    public ValueOfGoodsCurrencyAddedManually: boolean;


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