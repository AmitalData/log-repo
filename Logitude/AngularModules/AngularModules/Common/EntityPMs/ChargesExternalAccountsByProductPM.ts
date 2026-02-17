
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';

export class ChargesExternalAccountsByProductPM {
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

    private payablesGLAccount: string;
    public get PayablesGLAccount() { return this.payablesGLAccount; }
    public set PayablesGLAccount(newValue: string) { if (this.payablesGLAccount != newValue) { this.payablesGLAccount = newValue; this.MarkAsDirty("PayablesGLAccount"); } }

    private payablesCostCenter: string;
    public get PayablesCostCenter() { return this.payablesCostCenter; }
    public set PayablesCostCenter(newValue: string) { if (this.payablesCostCenter != newValue) { this.payablesCostCenter = newValue; this.MarkAsDirty("PayablesCostCenter"); } }

    private receivablesGLAccount: string;
    public get ReceivablesGLAccount() { return this.receivablesGLAccount; }
    public set ReceivablesGLAccount(newValue: string) { if (this.receivablesGLAccount != newValue) { this.receivablesGLAccount = newValue; this.MarkAsDirty("ReceivablesGLAccount"); } }

    private receivablesCostCenter: string;
    public get ReceivablesCostCenter() { return this.receivablesCostCenter; }
    public set ReceivablesCostCenter(newValue: string) { if (this.receivablesCostCenter != newValue) { this.receivablesCostCenter = newValue; this.MarkAsDirty("ReceivablesCostCenter"); } }

    private updateDate: Date;
    public get UpdateDate() { return this.updateDate; }
    public set UpdateDate(newValue: Date) { if (this.updateDate != newValue) { this.updateDate = newValue; this.MarkAsDirty("UpdateDate"); } }

    private chargesTypeId: string;
    public get ChargesTypeId() { return this.chargesTypeId; }
    public set ChargesTypeId(newValue: string) { if (this.chargesTypeId != newValue) { this.chargesTypeId = newValue; this.MarkAsDirty("ChargesTypeId"); } }

    private productTypeCode: string;
    public get ProductTypeCode() { return this.productTypeCode; }
    public set ProductTypeCode(newValue: string) { if (this.productTypeCode != newValue) { this.productTypeCode = newValue; this.MarkAsDirty("ProductTypeCode"); } }

    private updatedByUserId: string;
    public get UpdatedByUserId() { return this.updatedByUserId; }
    public set UpdatedByUserId(newValue: string) { if (this.updatedByUserId != newValue) { this.updatedByUserId = newValue; this.MarkAsDirty("UpdatedByUserId"); } }

    private productTypeName: string;
    public get ProductTypeName() { return this.productTypeName; }
    public set ProductTypeName(newValue: string) { if (this.productTypeName != newValue) { this.productTypeName = newValue; this.MarkAsDirty("ProductTypeName"); } }

    private updatedByUserName: string;
    public get UpdatedByUserName() { return this.updatedByUserName; }
    public set UpdatedByUserName(newValue: string) { if (this.updatedByUserName != newValue) { this.updatedByUserName = newValue; this.MarkAsDirty("UpdatedByUserName"); } }

    public OldEntityPM: ChargesExternalAccountsByProductPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "ChargesExternalAccountsByProduct");

        }
    }
    private MyClone: ChargesExternalAccountsByProductPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}