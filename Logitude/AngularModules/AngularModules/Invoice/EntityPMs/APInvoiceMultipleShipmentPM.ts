import {Output, EventEmitter}  from '@angular/core';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';

export class APInvoiceMultipleShipmentPM {
    public UIProperties: UIProperties;
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();

    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties;
        this.IsDirty = false;
    }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private aPInvoiceId: string;
    public get APInvoiceId() { return this.aPInvoiceId; }
    public set APInvoiceId(newValue: string) { if (this.aPInvoiceId != newValue) { this.aPInvoiceId = newValue; this.MarkAsDirty("APInvoiceId"); } }

    private shipmentId: string;
    public get ShipmentId() { return this.shipmentId; }
    public set ShipmentId(newValue: string) { if (this.shipmentId != newValue) { this.shipmentId = newValue; this.MarkAsDirty("ShipmentId"); } }

    private shipmentNumber: string;
    public get ShipmentNumber() { return this.shipmentNumber; }
    public set ShipmentNumber(newValue: string) { if (this.shipmentNumber != newValue) { this.shipmentNumber = newValue; this.MarkAsDirty("ShipmentNumber"); } }

    private shipmentLevelCode: string;
    public get ShipmentLevelCode() { return this.shipmentLevelCode; }
    public set ShipmentLevelCode(newValue: string) { if (this.shipmentLevelCode != newValue) { this.shipmentLevelCode = newValue; this.MarkAsDirty("ShipmentLevelCode"); } }

    private house: string;
    public get House() { return this.house; }
    public set House(newValue: string) { if (this.house != newValue) { this.house = newValue; this.MarkAsDirty("House"); } }

    private master: string;
    public get Master() { return this.master; }
    public set Master(newValue: string) { if (this.master != newValue) { this.master = newValue; this.MarkAsDirty("Master"); } }

    private longMaster: string;
    public get LongMaster() { return this.longMaster; }
    public set LongMaster(newValue: string) { if (this.longMaster != newValue) { this.longMaster = newValue; this.MarkAsDirty("LongMaster"); } }

    private mainCarriageCarrierName: string;
    public get MainCarriageCarrierName() { return this.mainCarriageCarrierName; }
    public set MainCarriageCarrierName(newValue: string) { if (this.mainCarriageCarrierName != newValue) { this.mainCarriageCarrierName = newValue; this.MarkAsDirty("MainCarriageCarrierName"); } }

    private partnerType: string;
    public get PartnerType() { return this.partnerType; }
    public set PartnerType(newValue: string) { if (this.partnerType != newValue) { this.partnerType = newValue; this.MarkAsDirty("PartnerType"); } }

    private partnerName: string;
    public get PartnerName() { return this.partnerName; }
    public set PartnerName(newValue: string) { if (this.partnerName != newValue) { this.partnerName = newValue; this.MarkAsDirty("PartnerName"); } }

    private subTotalInLocalCurrency: number;
    public get SubTotalInLocalCurrency() { return this.subTotalInLocalCurrency; }
    public set SubTotalInLocalCurrency(newValue: number) { if (this.subTotalInLocalCurrency != newValue) { this.subTotalInLocalCurrency = newValue; this.MarkAsDirty("SubTotalInLocalCurrency"); } }

    private subTotalInInvoiceCurrency: number;
    public get SubTotalInInvoiceCurrency() { return this.subTotalInInvoiceCurrency; }
    public set SubTotalInInvoiceCurrency(newValue: number) { if (this.subTotalInInvoiceCurrency != newValue) { this.subTotalInInvoiceCurrency = newValue; this.MarkAsDirty("SubTotalInInvoiceCurrency"); } }

    private expectedAmount: number;
    public get ExpectedAmount() { return this.expectedAmount; }
    public set ExpectedAmount(newValue: number) { if (this.expectedAmount != newValue) { this.expectedAmount = newValue; this.MarkAsDirty("ExpectedAmount"); } }

    private accountedAmount: number;
    public get AccountedAmount() { return this.accountedAmount; }
    public set AccountedAmount(newValue: number) { if (this.accountedAmount != newValue) { this.accountedAmount = newValue; this.MarkAsDirty("AccountedAmount"); } }

    private openAmount: number;
    public get OpenAmount() { return this.openAmount; }
    public set OpenAmount(newValue: number) { if (this.openAmount != newValue) { this.openAmount = newValue; this.MarkAsDirty("OpenAmount"); } }

    private totalAmount: number;
    public get TotalAmount() { return this.totalAmount; }
    public set TotalAmount(newValue: number) { if (this.totalAmount != newValue) { this.totalAmount = newValue; this.MarkAsDirty("TotalAmount"); } }

    private totalVATAmount: number;
    public get TotalVATAmount() { return this.totalVATAmount; }
    public set TotalVATAmount(newValue: number) { if (this.totalVATAmount != newValue) { this.totalVATAmount = newValue; this.MarkAsDirty("TotalVATAmount"); } }

    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { if (this.indexOrder != newValue) { this.indexOrder = newValue; this.MarkAsDirty("IndexOrder"); } }

    private totalVatsList: string[];
    get TotalVatsList() {
        if (this.totalVatsList == null) {
            this.totalVatsList = [];
        }

        return this.totalVatsList;
    }
    set TotalVatsList(newValue: string[]) {
        if (this.totalVatsList != newValue) {
            this.totalVatsList = newValue;
        }
    }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { this.changeSetOp = newValue; this.MarkAsDirty(); }

    public OldEntityPM: APInvoiceMultipleShipmentPM;

    private entityParentPM: any;
    public get EntityParentPM() { return this.entityParentPM; }
    public set EntityParentPM(newValue: any) { this.entityParentPM = newValue; }

    public UniqueKey: string;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (this.EntityParentPM) {
            this.EntityParentPM.MarkAsDirty();
        }

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceMultipleShipment");
        }
    }
    private MyClone: APInvoiceMultipleShipmentPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}