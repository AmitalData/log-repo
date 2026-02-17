import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
import {CustomFieldClass} from '../../Infrastructure/DataContracts/CustomFieldClass';

export class INTTRASettingPM {
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

    private outSettingsId: string;
    public get OutSettingsId() { return this.outSettingsId; }
    public set OutSettingsId(newValue: string) { if (this.outSettingsId != newValue) { this.outSettingsId = newValue; this.MarkAsDirty("OutSettingsId"); } }

    private outSettingsHost: string;
    public get OutSettingsHost() { return this.outSettingsHost; }
    public set OutSettingsHost(newValue: string) { if (this.outSettingsHost != newValue) { this.outSettingsHost = newValue; this.MarkAsDirty("OutSettingsHost"); } }

    private inSettingsId: string;
    public get InSettingsId() { return this.inSettingsId; }
    public set InSettingsId(newValue: string) { if (this.inSettingsId != newValue) { this.inSettingsId = newValue; this.MarkAsDirty("InSettingsId"); } }

    private inSettingsHost: string;
    public get InSettingsHost() { return this.inSettingsHost; }
    public set InSettingsHost(newValue: string) { if (this.inSettingsHost != newValue) { this.inSettingsHost = newValue; this.MarkAsDirty("InSettingsHost"); } }

    private iNTTRASettingModeCode: string;
    public get INTTRASettingModeCode() { return this.iNTTRASettingModeCode; }
    public set INTTRASettingModeCode(newValue: string) { if (this.iNTTRASettingModeCode != newValue) { this.iNTTRASettingModeCode = newValue; this.MarkAsDirty("INTTRASettingModeCode"); } }

    private iNTTRAId: string;
    public get INTTRAId() { return this.iNTTRAId; }
    public set INTTRAId(newValue: string) { if (this.iNTTRAId != newValue) { this.iNTTRAId = newValue; this.MarkAsDirty("INTTRAId"); } }

    private iNTTRAAlias: string;
    public get INTTRAAlias() { return this.iNTTRAAlias; }
    public set INTTRAAlias(newValue: string) { if (this.iNTTRAAlias != newValue) { this.iNTTRAAlias = newValue; this.MarkAsDirty("INTTRAAlias"); } }

    public OldEntityPM: INTTRASettingPM;

    public IsDirty: boolean;
    MarkAsDirty(propertyName: string = null) {
        this.IsDirty = true;

        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName, this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "INTTRASetting");

        }
    }
    private MyClone: INTTRASettingPM;

    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }
}