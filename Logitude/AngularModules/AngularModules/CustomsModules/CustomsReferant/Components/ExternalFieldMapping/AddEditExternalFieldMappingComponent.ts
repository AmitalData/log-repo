import { Component, OnInit } from '@angular/core';
import { ExternalFieldMappingPM } from 'Customs/EntityPMs/ExternalFieldMappingPM';
import { GTBFUSTATUWebService } from 'Customs/Services/WebServices/GTBFUSTATUWebService';
import { BaseComponent } from "Infrastructure/Components/LogitudeComponents/BaseComponent";
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { AppTool } from 'Infrastructure/Tools';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';


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


    constructor(public entityArgs: EntityArgs) {
        super();
        this._EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this._GTBFUSTATUWebService.GetAllGTBFUSTATU().subscribe((statusList: any) => {
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
        });
    }
    SetWindowArgs(args: any) {

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
    }

    public get StatusName() { return this.EntityPM.StatusName }
    public set StatusName(newValue: string) {
        this.EntityPM.StatusName = newValue;
    }

    ngOnInit() {
    }

    OkButtonClicked() {

    }
    CancelButtonClicked() {
        SessionLocator.SelectedSession.CloseCurrentWindow();
    }
}