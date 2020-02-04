
import {ShipmentPM} from './ShipmentPM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class ConsoleShipmentPM {
    public UIProperties: UIProperties;
    constructor( entityParentPM: ShipmentPM) {
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; this.MarkAsDirty(); }

    private isLCL: boolean;
    public get IsLCL() { return this.isLCL; }
    public set IsLCL(newValue: boolean) { this.isLCL = newValue; this.MarkAsDirty(); }

    private isFCL: boolean;
    public get IsFCL() { return this.isFCL; }
    public set IsFCL(newValue: boolean) { this.isFCL = newValue; this.MarkAsDirty(); }

    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(newValue: string) { this.shipmentNumber = newValue; this.MarkAsDirty(); }

    private masterShipmentDataId: string;
    public get MasterShipmentDataId() { return this.masterShipmentDataId; }
    public set MasterShipmentDataId(newValue: string) { this.masterShipmentDataId = newValue; this.MarkAsDirty(); }

    private fHLStatusCode: string;
    public get FHLStatusCode() { return this.fHLStatusCode; }
    public set FHLStatusCode(newValue: string) { this.fHLStatusCode = newValue; this.MarkAsDirty(); }

    private fHLStatusName: string;
    public get FHLStatusName() { return this.fHLStatusName; }
    public set FHLStatusName(newValue: string) { this.fHLStatusName = newValue; this.MarkAsDirty(); }

    private cargonautFHLStatusCode: string;
    public get CargonautFHLStatusCode() { return this.cargonautFHLStatusCode; }
    public set CargonautFHLStatusCode(newValue: string) { this.cargonautFHLStatusCode = newValue; this.MarkAsDirty(); }

    private cargonautFHLStatusName: string;
    public get CargonautFHLStatusName() { return this.cargonautFHLStatusName; }
    public set CargonautFHLStatusName(newValue: string) { this.cargonautFHLStatusName = newValue; this.MarkAsDirty(); }

    private fNAReason: string;
    public get FNAReason() { return this.fNAReason; }
    public set FNAReason(newValue: string) { this.fNAReason = newValue; this.MarkAsDirty(); }

    private tEU: number;
    public get TEU() { return this.tEU; }
    public set TEU(newValue: number) { this.tEU = newValue; this.MarkAsDirty(); }

    private volume: number;
    public get Volume() { return this.volume; }
    public set Volume(newValue: number) { this.volume = newValue; this.MarkAsDirty(); }

    private grossWeight: number;
    public get GrossWeight() { return this.grossWeight; }
    public set GrossWeight(newValue: number) { this.grossWeight = newValue; this.MarkAsDirty(); }

    private grossWeightInKG: number;
    public get GrossWeightInKG() { return this.grossWeightInKG; }
    public set GrossWeightInKG(newValue: number) { this.grossWeightInKG = newValue; this.MarkAsDirty(); } 

    private grossWeightPerStorageDays: number;
    public get GrossWeightPerStorageDays() { return this.grossWeightPerStorageDays; }
    public set GrossWeightPerStorageDays(newValue: number) { if (this.grossWeightPerStorageDays != newValue) { this.grossWeightPerStorageDays = newValue; this.MarkAsDirty("GrossWeightPerStorageDays"); } }


    private grossWeightPerTon: number;
    public get GrossWeightPerTon() { return this.grossWeightPerTon; }
    public set GrossWeightPerTon(newValue: number) { this.grossWeightPerTon = newValue; this.MarkAsDirty(); } 

    private chargeableWeight: number;
    public get ChargeableWeight() { return this.chargeableWeight; }
    public set ChargeableWeight(newValue: number) { this.chargeableWeight = newValue; this.MarkAsDirty(); }

    private chargeableWeightInKG: number;
    public get ChargeableWeightInKG() { return this.chargeableWeightInKG; }
    public set ChargeableWeightInKG(newValue: number) { if (this.chargeableWeightInKG != newValue) { this.chargeableWeightInKG = newValue; this.MarkAsDirty(); } }

    private volumetricWeight: number;
    public get VolumetricWeight() { return this.volumetricWeight; }
    public set VolumetricWeight(newValue: number) { this.volumetricWeight = newValue; this.MarkAsDirty(); }

    private oAMTPayables_Local: number;
    public get OAMTPayables_Local() { return this.oAMTPayables_Local; }
    public set OAMTPayables_Local(newValue: number) { this.oAMTPayables_Local = newValue; this.MarkAsDirty(); }

    private aCCTPayables_Local: number;
    public get ACCTPayables_Local() { return this.aCCTPayables_Local; }
    public set ACCTPayables_Local(newValue: number) { this.aCCTPayables_Local = newValue; this.MarkAsDirty(); }

    private oAMTPayables_Profit: number;
    public get OAMTPayables_Profit() { return this.oAMTPayables_Profit; }
    public set OAMTPayables_Profit(newValue: number) { this.oAMTPayables_Profit = newValue; this.MarkAsDirty(); }

    private aCCTPayables_Profit: number;
    public get ACCTPayables_Profit() { return this.aCCTPayables_Profit; }
    public set ACCTPayables_Profit(newValue: number) { this.aCCTPayables_Profit = newValue; this.MarkAsDirty(); }

    private oAMTReceivables_Local: number;
    public get OAMTReceivables_Local() { return this.oAMTReceivables_Local; }
    public set OAMTReceivables_Local(newValue: number) { this.oAMTReceivables_Local = newValue; this.MarkAsDirty(); }

    private aCCTReceivables_Local: number;
    public get ACCTReceivables_Local() { return this.aCCTReceivables_Local; }
    public set ACCTReceivables_Local(newValue: number) { this.aCCTReceivables_Local = newValue; this.MarkAsDirty(); }

    private oAMTReceivables_Profit: number;
    public get OAMTReceivables_Profit() { return this.oAMTReceivables_Profit; }
    public set OAMTReceivables_Profit(newValue: number) { this.oAMTReceivables_Profit = newValue; this.MarkAsDirty(); }

    private aCCTReceivables_Profit: number;
    public get ACCTReceivables_Profit() { return this.aCCTReceivables_Profit; }
    public set ACCTReceivables_Profit(newValue: number) { this.aCCTReceivables_Profit = newValue; this.MarkAsDirty(); }

    private oAMTReceivables_Local_NoParent: number;
    public get OAMTReceivables_Local_NoParent() { return this.oAMTReceivables_Local_NoParent; }
    public set OAMTReceivables_Local_NoParent(newValue: number) { this.oAMTReceivables_Local_NoParent = newValue; this.MarkAsDirty(); }

    private aCCTReceivables_Local_NoParent: number;
    public get ACCTReceivables_Local_NoParent() { return this.aCCTReceivables_Local_NoParent; }
    public set ACCTReceivables_Local_NoParent(newValue: number) { this.aCCTReceivables_Local_NoParent = newValue; this.MarkAsDirty(); }

    private oAMTReceivables_Profit_NoParent: number;
    public get OAMTReceivables_Profit_NoParent() { return this.oAMTReceivables_Profit_NoParent; }
    public set OAMTReceivables_Profit_NoParent(newValue: number) { this.oAMTReceivables_Profit_NoParent = newValue; this.MarkAsDirty(); }

    private aCCTReceivables_Profit_NoParent: number;
    public get ACCTReceivables_Profit_NoParent() { return this.aCCTReceivables_Profit_NoParent; }
    public set ACCTReceivables_Profit_NoParent(newValue: number) { this.aCCTReceivables_Profit_NoParent = newValue; this.MarkAsDirty(); }

    private valueOfGoods: number;
    public get ValueOfGoods() { return this.valueOfGoods; }
    public set ValueOfGoods(newValue: number) { this.valueOfGoods = newValue; this.MarkAsDirty(); }

    private freightPayablesAmount: number;
    public get FreightPayablesAmount() { return this.freightPayablesAmount; }
    public set FreightPayablesAmount(newValue: number) { this.freightPayablesAmount = newValue; this.MarkAsDirty(); }

    private freightReceivablesAmount: number;
    public get FreightReceivablesAmount() { return this.freightReceivablesAmount; }
    public set FreightReceivablesAmount(newValue: number) { this.freightReceivablesAmount = newValue; this.MarkAsDirty(); }

    private numberOfPackages: number;
    public get NumberOfPackages() { return this.numberOfPackages; }
    public set NumberOfPackages(newValue: number) { this.numberOfPackages = newValue; this.MarkAsDirty(); } 

    private numberOfContainers: number;
    public get NumberOfContainers() { return this.numberOfContainers; }
    public set NumberOfContainers(newValue: number) { this.numberOfContainers = newValue; this.MarkAsDirty(); } 

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    private volumeInCBM: number;
    public get VolumeInCBM() { return this.volumeInCBM; }
    public set VolumeInCBM(newValue: number) { this.volumeInCBM = newValue; this.MarkAsDirty(); }


    private fCLDataList: HouseContainerPackage[];
    get FCLDataList() {
        if (this.fCLDataList == null) {
            this.fCLDataList = [];
        }

        return this.fCLDataList;
    }
    set FCLDataList(newValue: HouseContainerPackage[]) {
        if (this.fCLDataList != newValue) {
            this.fCLDataList = newValue;
        }
    }   

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public OldEntityPM: ConsoleShipmentPM;

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
        if (this.entityParentPM) {
            this.entityParentPM.MarkAsDirty();
        }
    }
}

class HouseContainerPackage {

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }

    private consoleId: string;
    public get ConsoleId() { return this.consoleId; }
    public set ConsoleId(newValue: string) { this.consoleId = newValue; }

    private quantity: number;
    public get Quantity() { return this.quantity; }
    public set Quantity(newValue: number) { this.quantity = newValue; }
}
