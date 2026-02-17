import {Component, OnInit} from '@angular/core';
import {CreditLimitSettingPM} from '../../../EntityPMs/CreditLimitSettingPM';
import {CreditLimitSettingPMService} from '../../../Services/StandardPMs/CreditLimitSettingPMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './CreditLimitSettingsComponent.html',
})

export class CreditLimitSettingsComponent extends BaseComponent implements OnInit {
    public EntityPM: CreditLimitSettingPM;
    public ObjectTableName: string = "CreditLimitSetting";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public IsResourcesReady: boolean = false;
    private myService: CreditLimitSettingPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        this.myService = new CreditLimitSettingPMService();
    }

    ngOnInit() {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.myService.get(SessionLocator.Tenant + "").subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    if (myResponse.Result) {
                        this.EntityPM = myResponse.Result;
                    }

                    else {
                        this.EntityPM = new CreditLimitSettingPM();
                        this.EntityPM.Tenant = SessionLocator.Tenant;
                        this.EntityPM.IsDirty = false;                        
                    }

                    this.SetUIProperties();
                    this.IsResourcesReady = true;
                }
            });
        });
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled('InvoiceCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('InvoiceCreationWarning', this.ObjectTableName, this.IsCreditLimitEnabled);
        this.UIProperties.SetEnabled('ShipmentCreationBlock', this.ObjectTableName, this.IsCreditLimitEnabled);
    }

    get IsCreditLimitEnabled() { return this.EntityPM.IsCreditLimitEnabled; }
    set IsCreditLimitEnabled(value: boolean) {
        if (this.EntityPM.IsCreditLimitEnabled != value) {
            this.EntityPM.IsCreditLimitEnabled = value;
            this.SetUIProperties();
        }
    }

    get InvoiceCreationBlock() { return this.EntityPM.InvoiceCreationBlock; }
    set InvoiceCreationBlock(value: boolean) {
        if (this.EntityPM.InvoiceCreationBlock != value) {
            this.EntityPM.InvoiceCreationBlock = value;
        }
    }

    get InvoiceCreationWarning() { return this.EntityPM.InvoiceCreationWarning; }
    set InvoiceCreationWarning(value: boolean) {
        if (this.EntityPM.InvoiceCreationWarning != value) {
            this.EntityPM.InvoiceCreationWarning = value;
        }
    }

    get ShipmentCreationBlock() { return this.EntityPM.ShipmentCreationBlock; }
    set ShipmentCreationBlock(value: boolean) {
        if (this.EntityPM.ShipmentCreationBlock != value) {
            this.EntityPM.ShipmentCreationBlock = value;
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        if (this.EntityPM.IsDirty) {
            this.CurrentSession.StartBusyIndicatorSaving();

            if (this.EntityPM.Id == null) {
                this.myService.insert(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();

                    if (!myRespone.HasError) {
                        ObjectsLocator.CreditLimitSettingPM = this.EntityPM;
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }

            else {
                this.myService.update(this.EntityPM).subscribe((myRespone: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();

                    if (!myRespone.HasError) {
                        ObjectsLocator.CreditLimitSettingPM = this.EntityPM;
                        this.CurrentSession.CloseCurrentWindowEmit("OK");
                    }

                    else {
                        this.ValidationErrorsList = myRespone.ErrorsArray;
                    }
                });
            }
        }

        else {
            this.CurrentSession.CloseCurrentWindow();
        }        
    }
}
