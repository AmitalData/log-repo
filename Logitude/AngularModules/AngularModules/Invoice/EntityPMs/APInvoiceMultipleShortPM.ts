import {Output, EventEmitter}  from '@angular/core';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {APInvoiceLinePM} from './APInvoiceLinePM';

export class APInvoiceMultipleShortPM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;
    constructor() {
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { if (this.id != newValue) { this.id = newValue; this.MarkAsDirty("Id"); } }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private shipmentId: string;
    public get ShipmentId() { return this.shipmentId; }
    public set ShipmentId(newValue: string) { if (this.shipmentId != newValue) { this.shipmentId = newValue; this.MarkAsDirty("ShipmentId"); } }

    private statusCode: string;
    public get StatusCode() { return this.statusCode; }
    public set StatusCode(newValue: string) { if (this.statusCode != newValue) { this.statusCode = newValue; this.MarkAsDirty("StatusCode"); } }

    private vendorId: string;
    public get VendorId() { return this.vendorId; }
    public set VendorId(newValue: string) { if (this.vendorId != newValue) { this.vendorId = newValue; this.MarkAsDirty("VendorId"); } }

    private profitCurrencyId: string;
    public get ProfitCurrencyId() { return this.profitCurrencyId; }
    public set ProfitCurrencyId(newValue: string) { if (this.profitCurrencyId != newValue) { this.profitCurrencyId = newValue; this.MarkAsDirty("ProfitCurrencyId"); } }

    private invoiceCurrencyId: string;
    public get InvoiceCurrencyId() { return this.invoiceCurrencyId; }
    public set InvoiceCurrencyId(newValue: string) { if (this.invoiceCurrencyId != newValue) { this.invoiceCurrencyId = newValue; this.MarkAsDirty("InvoiceCurrencyId"); } }

    private invoiceCurrencyExchangeRate: number;
    public get InvoiceCurrencyExchangeRate() { return this.invoiceCurrencyExchangeRate; }
    public set InvoiceCurrencyExchangeRate(newValue: number) { if (this.invoiceCurrencyExchangeRate != newValue) { this.invoiceCurrencyExchangeRate = newValue; this.MarkAsDirty("InvoiceCurrencyExchangeRate"); } }

    private profitCurrencyExchangeRate: number;
    public get ProfitCurrencyExchangeRate() { return this.profitCurrencyExchangeRate; }
    public set ProfitCurrencyExchangeRate(newValue: number) { if (this.profitCurrencyExchangeRate != newValue) { this.profitCurrencyExchangeRate = newValue; this.MarkAsDirty("ProfitCurrencyExchangeRate"); } }

    private subTotalInLocalCurrency: number;
    public get SubTotalInLocalCurrency() { return this.subTotalInLocalCurrency; }
    public set SubTotalInLocalCurrency(newValue: number) { if (this.subTotalInLocalCurrency != newValue) { this.subTotalInLocalCurrency = newValue; this.MarkAsDirty("SubTotalInLocalCurrency"); } }

    private subTotalInInvoiceCurrency: number;
    public get SubTotalInInvoiceCurrency() { return this.subTotalInInvoiceCurrency; }
    public set SubTotalInInvoiceCurrency(newValue: number) { if (this.subTotalInInvoiceCurrency != newValue) { this.subTotalInInvoiceCurrency = newValue; this.MarkAsDirty("SubTotalInInvoiceCurrency"); } }

    private isMultipleEntities: boolean = true;
    public get IsMultipleEntities() { return this.isMultipleEntities; }
    public set IsMultipleEntities(newValue: boolean) { if (this.isMultipleEntities != newValue) { this.isMultipleEntities = newValue; this.MarkAsDirty("IsMultipleEntities"); } }

    private invoiceLines: APInvoiceLinePM[];
    get InvoiceLines() {
        if (this.invoiceLines == null) {
            this.invoiceLines = [];
        }

        return this.invoiceLines;
    }
    set InvoiceLines(newValue: APInvoiceLinePM[]) {
        if (this.invoiceLines != newValue) {
            this.invoiceLines = newValue;
        }
    }
    public AddInvoiceLinePM(item: APInvoiceLinePM) {
        if (item != null) {
            var index = this.InvoiceLines.indexOf(item);
            if (index == -1) {
                item.EntityParentPM = this;
                this.InvoiceLines.push(item);
                this.MarkAsDirty();
            }
        }
    }
    public RemoveInvoiceLinePM(item: APInvoiceLinePM) {
        if (item != null) {
            var index = this.InvoiceLines.indexOf(item);
            if (index > -1) {
                this.InvoiceLines.splice(index, 1);
                this.MarkAsDirty();
            }
        }
    }

    public OldEntityPM: APInvoiceMultipleShortPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceMultipleShort");

        }
    }

    private MyClone: APInvoiceMultipleShortPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}
export class APInvoiceLineShortPM {
    @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
    public UIProperties: UIProperties;

    constructor(_entityParentPM: any) {
        this.EntityParentPM = _entityParentPM;
        this.UIProperties = new UIProperties(this);
        this.IsDirty = false;
    }

    private aPInvoiceId: string;
    public get APInvoiceId() { return this.aPInvoiceId; }
    public set APInvoiceId(newValue: string) { if (this.aPInvoiceId != newValue) { this.aPInvoiceId = newValue; this.MarkAsDirty("APInvoiceId"); } }

    private lineNumber: number;
    public get LineNumber() { return this.lineNumber; }
    public set LineNumber(newValue: number) { if (this.lineNumber != newValue) { this.lineNumber = newValue; this.MarkAsDirty("LineNumber"); } }

    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { if (this.tenant != newValue) { this.tenant = newValue; this.MarkAsDirty("Tenant"); } }

    private invoiceCurrencyAmount: number;
    public get InvoiceCurrencyAmount() { return this.invoiceCurrencyAmount; }
    public set InvoiceCurrencyAmount(newValue: number) { if (this.invoiceCurrencyAmount != newValue) { this.invoiceCurrencyAmount = newValue; this.MarkAsDirty("InvoiceCurrencyAmount"); } }

    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) { if (this.notes != newValue) { this.notes = newValue; this.MarkAsDirty("Notes"); } }

    private chargesTypeId: string;
    public get ChargesTypeId() { return this.chargesTypeId; }
    public set ChargesTypeId(newValue: string) { if (this.chargesTypeId != newValue) { this.chargesTypeId = newValue; this.MarkAsDirty("ChargesTypeId"); } }

    private vatTypeId: string;
    public get VatTypeId() { return this.vatTypeId; }
    public set VatTypeId(newValue: string) { if (this.vatTypeId != newValue) { this.vatTypeId = newValue; this.MarkAsDirty("VatTypeId"); } }

    private vendorId: string;
    public get VendorId() { return this.vendorId; }
    public set VendorId(newValue: string) { if (this.vendorId != newValue) { this.vendorId = newValue; this.MarkAsDirty("VendorId"); } }

    private openAmount: number;
    public get OpenAmount() { return this.openAmount; }
    public set OpenAmount(newValue: number) { if (this.openAmount != newValue) { this.openAmount = newValue; this.MarkAsDirty("OpenAmount"); } }

    private foriegnCurrencyId: string;
    public get ForiegnCurrencyId() { return this.foriegnCurrencyId; }
    public set ForiegnCurrencyId(newValue: string) { if (this.foriegnCurrencyId != newValue) { this.foriegnCurrencyId = newValue; this.MarkAsDirty("ForiegnCurrencyId"); } }

    private foriegnCurrencyAmount: number;
    public get ForiegnCurrencyAmount() { return this.foriegnCurrencyAmount; }
    public set ForiegnCurrencyAmount(newValue: number) { if (this.foriegnCurrencyAmount != newValue) { this.foriegnCurrencyAmount = newValue; this.MarkAsDirty("ForiegnCurrencyAmount"); } }

    private foriegnExchangeRate: number;
    public get ForiegnExchangeRate() { return this.foriegnExchangeRate; }
    public set ForiegnExchangeRate(newValue: number) { if (this.foriegnExchangeRate != newValue) { this.foriegnExchangeRate = newValue; this.MarkAsDirty("ForiegnExchangeRate"); } }

    private localCurrencyAmount: number;
    public get LocalCurrencyAmount() { return this.localCurrencyAmount; }
    public set LocalCurrencyAmount(newValue: number) { if (this.localCurrencyAmount != newValue) { this.localCurrencyAmount = newValue; this.MarkAsDirty("LocalCurrencyAmount"); } }

    private profitCurrencyAmount: number;
    public get ProfitCurrencyAmount() { return this.profitCurrencyAmount; }
    public set ProfitCurrencyAmount(newValue: number) { if (this.profitCurrencyAmount != newValue) { this.profitCurrencyAmount = newValue; this.MarkAsDirty("ProfitCurrencyAmount"); } }

    private entityId: string;
    public get EntityId() { return this.entityId; }
    public set EntityId(newValue: string) { if (this.entityId != newValue) { this.entityId = newValue; this.MarkAsDirty("EntityId"); } }

    private entityPayableId: string;
    public get EntityPayableId() { return this.entityPayableId; }
    public set EntityPayableId(newValue: string) { if (this.entityPayableId != newValue) { this.entityPayableId = newValue; this.MarkAsDirty("EntityPayableId"); } }

    private refundAmount: number;
    public get RefundAmount() { return this.refundAmount; }
    public set RefundAmount(newValue: number) { if (this.refundAmount != newValue) { this.refundAmount = newValue; this.MarkAsDirty("RefundAmount"); } }

    private chargesTypeCode: string;
    public get ChargesTypeCode() { return this.chargesTypeCode; }
    public set ChargesTypeCode(newValue: string) { if (this.chargesTypeCode != newValue) { this.chargesTypeCode = newValue; this.MarkAsDirty("ChargesTypeCode"); } }

    private chargesTypeName: string;
    public get ChargesTypeName() { return this.chargesTypeName; }
    public set ChargesTypeName(newValue: string) { if (this.chargesTypeName != newValue) { this.chargesTypeName = newValue; this.MarkAsDirty("ChargesTypeName"); } }

    private vatTypeName: string;
    public get VatTypeName() { return this.vatTypeName; }
    public set VatTypeName(newValue: string) { if (this.vatTypeName != newValue) { this.vatTypeName = newValue; this.MarkAsDirty("VatTypeName"); } }

    private objectTableId: string;
    public get ObjectTableId() { return this.objectTableId; }
    public set ObjectTableId(newValue: string) { if (this.objectTableId != newValue) { this.objectTableId = newValue; this.MarkAsDirty("ObjectTableId"); } }

    private entityReference: string;
    public get EntityReference() { return this.entityReference; }
    public set EntityReference(newValue: string) { if (this.entityReference != newValue) { this.entityReference = newValue; this.MarkAsDirty("EntityReference"); } }

    private vendorName: string;
    public get VendorName() { return this.vendorName; }
    public set VendorName(newValue: string) { if (this.vendorName != newValue) { this.vendorName = newValue; this.MarkAsDirty("VendorName"); } }

    private otherInvoicesAmounts: number;
    public get OtherInvoicesAmounts() { return this.otherInvoicesAmounts; }
    public set OtherInvoicesAmounts(newValue: number) { if (this.otherInvoicesAmounts != newValue) { this.otherInvoicesAmounts = newValue; this.MarkAsDirty("OtherInvoicesAmounts"); } }

    private expectedAmount: number;
    public get ExpectedAmount() { return this.expectedAmount; }
    public set ExpectedAmount(newValue: number) { if (this.expectedAmount != newValue) { this.expectedAmount = newValue; this.MarkAsDirty("ExpectedAmount"); } }

    private correctionAmount: number;
    public get CorrectionAmount() { return this.correctionAmount; }
    public set CorrectionAmount(newValue: number) { if (this.correctionAmount != newValue) { this.correctionAmount = newValue; this.MarkAsDirty("CorrectionAmount"); } }

    private correctionNote: string;
    public get CorrectionNote() { return this.correctionNote; }
    public set CorrectionNote(newValue: string) { if (this.correctionNote != newValue) { this.correctionNote = newValue; this.MarkAsDirty("CorrectionNote"); } }

    private correctionByUserId: string;
    public get CorrectionByUserId() { return this.correctionByUserId; }
    public set CorrectionByUserId(newValue: string) { if (this.correctionByUserId != newValue) { this.correctionByUserId = newValue; this.MarkAsDirty("CorrectionByUserId"); } }

    private correctionDate: Date;
    public get CorrectionDate() { return this.correctionDate; }
    public set CorrectionDate(newValue: Date) { if (this.correctionDate != newValue) { this.correctionDate = newValue; this.MarkAsDirty("CorrectionDate"); } }

    private amountTypeCode: string;
    public get AmountTypeCode() { return this.amountTypeCode; }
    public set AmountTypeCode(newValue: string) { if (this.amountTypeCode != newValue) { this.amountTypeCode = newValue; this.MarkAsDirty("AmountTypeCode"); } }

    private vatPercentage: number;
    public get VatPercentage() { return this.vatPercentage; }
    public set VatPercentage(newValue: number) { if (this.vatPercentage != newValue) { this.vatPercentage = newValue; this.MarkAsDirty("VatPercentage"); } }

    private foriegnCurrencyCode: string;
    public get ForiegnCurrencyCode() { return this.foriegnCurrencyCode; }
    public set ForiegnCurrencyCode(newValue: string) { if (this.foriegnCurrencyCode != newValue) { this.foriegnCurrencyCode = newValue; this.MarkAsDirty("ForiegnCurrencyCode"); } }

    private changeSetOp: string;
    public get ChangeSetOp() { return this.changeSetOp; }
    public set ChangeSetOp(newValue: string) { if (this.changeSetOp != newValue) { this.changeSetOp = newValue; this.MarkAsDirty("ChangeSetOp"); } }

    private debitAccount: string;
    public get DebitAccount() { return this.debitAccount; }
    public set DebitAccount(newValue: string) { if (this.debitAccount != newValue) { this.debitAccount = newValue; this.MarkAsDirty("DebitAccount"); } }

    private description: string;
    public get Description() { return this.description; }
    public set Description(newValue: string) { if (this.description != newValue) { this.description = newValue; this.MarkAsDirty("Description"); } }

    private localDescription: string;
    public get LocalDescription() { return this.localDescription; }
    public set LocalDescription(newValue: string) { if (this.localDescription != newValue) { this.localDescription = newValue; this.MarkAsDirty("LocalDescription"); } }

    private externalVATCard: string;
    public get ExternalVATCard() { return this.externalVATCard; }
    public set ExternalVATCard(newValue: string) { if (this.externalVATCard != newValue) { this.externalVATCard = newValue; this.MarkAsDirty("ExternalVATCard"); } }

    private externalTAXItemId: string;
    public get ExternalTAXItemId() { return this.externalTAXItemId; }
    public set ExternalTAXItemId(newValue: string) { if (this.externalTAXItemId != newValue) { this.externalTAXItemId = newValue; this.MarkAsDirty("ExternalTAXItemId"); } }

    private chargeTypeGLAccountId: string;
    public get ChargeTypeGLAccountId() { return this.chargeTypeGLAccountId; }
    public set ChargeTypeGLAccountId(newValue: string) { if (this.chargeTypeGLAccountId != newValue) { this.chargeTypeGLAccountId = newValue; this.MarkAsDirty("ChargeTypeGLAccountId"); } }

    private authorizedSignatory: boolean;
    public get AuthorizedSignatory() { return this.authorizedSignatory; }
    public set AuthorizedSignatory(newValue: boolean) { if (this.authorizedSignatory != newValue) { this.authorizedSignatory = newValue; this.MarkAsDirty("AuthorizedSignatory"); } }

    public OldEntityPM: APInvoiceLineShortPM;

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
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "APInvoiceLine");

        }
    }
    private MyClone: APInvoiceLineShortPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}