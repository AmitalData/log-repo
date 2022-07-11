import { Component, OnInit } from '@angular/core';
import { ExternalFieldMappingPM } from 'Customs/EntityPMs/ExternalFieldMappingPM';
import { ExternalFieldMappingPMService } from 'Customs/Services/StandardPMs/ExternalFieldMappingPMService';
import { GTBFUSTATUWebService } from 'Customs/Services/WebServices/GTBFUSTATUWebService';
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Validator } from '../../../../Infrastructure/Validators/Validator';


@Component({
    templateUrl: './AddEditExternalFieldMappingComponent.html',
})
export class AddEditExternalFieldMappingComponent
    extends BaseComponent
    implements OnInit {


    public DataContext: any = this;
    public ObjectTableName: string = "Customs.ExternalFieldMapping";
    isWindowMode: boolean = false;
    isNewRecord: boolean = false;
    ValidationErrorsList: any[] = [];
    private _EntityResourceService: EntityResourceService = new EntityResourceService();
    private _GTBFUSTATUWebService: GTBFUSTATUWebService = new GTBFUSTATUWebService();
    Loaded: boolean = false;
    _ExternalFieldMappingPMService: ExternalFieldMappingPMService = new ExternalFieldMappingPMService();


    constructor(public entityArgs: EntityArgs) {
        super();
        debugger;
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this.Loaded = true;
            if (AppTool.IsNullOrEmpty(entityArgs.EntityPM) || !(entityArgs.EntityPM instanceof ExternalFieldMappingPM)) {
                this.EntityPM = new ExternalFieldMappingPM();
                this.EntityPM.Tenant = SessionLocator.Tenant;
                this.isWindowMode = true;
                this.isNewRecord = true;

            } else {
                this.EntityPM = this.entityArgs.EntityPM;
            }
        });
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("StatusName", this.ObjectTableName, false);
        if (!this.isWindowMode) {
            this.UIProperties.SetEnabled("StatusFieldType", this.ObjectTableName, false);
        }
    }
    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.isWindowMode = true;
            this.isNewRecord = true;
            this.EntityPM = new ExternalFieldMappingPM();
            this.EntityPM.Tenant = SessionLocator.Tenant;

        }
    }

    private _WarningMessage: string;
    public get WarningMessage() { return this._WarningMessage; }
    public set WarningMessage(newValue: string) {
        this._WarningMessage = newValue;
    }

    public get StatusFieldType() { return this.EntityPM.StatusFieldType; }
    public set StatusFieldType(newValue: string) {
        this.EntityPM.StatusFieldType = newValue;
    }

    public get StatusCode() { return this.EntityPM.StatusCode; }
    public set StatusCode(newValue: string) {
        this.EntityPM.StatusCode = newValue;
        this.EntityPM.StatusName = newValue;

    }

    public get StatusName() { return this.EntityPM.StatusName }
    public set StatusName(newValue: string) {
        this.EntityPM.StatusName = newValue;
    }

    public get InActive() { return this.EntityPM.InActive }
    public set InActive(newValue: string) {
        this.EntityPM.InActive = newValue;
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    OkButtonClicked() {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.isNewRecord) {
            this._ExternalFieldMappingPMService.insert(this.EntityPM).subscribe(myResult => {
                debugger;
                if (myResult.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                } else {
                    this.CancelButtonClicked();

                }
            });
        }
        else {
            this._ExternalFieldMappingPMService.update(this.EntityPM).subscribe(myResult => {
                if (myResult.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(myResult.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();
            });
        }

    }
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
}