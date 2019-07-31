import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {INTTRADomainService, INTTRACommunicationSettingsHelper} from '../../Services/INTTRADomainService';

@Component({
    moduleId: module.id,
    templateUrl: './INTTRACommunicationSettingsComponent.html',
})

export class INTTRACommunicationSettingsComponent extends BaseComponent {
    public EntityPM: INTTRACommunicationSettingsHelper = null;
    public ObjectTableName = "INTTRACommunicationSettings";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public ValidationErrorsList: string[] = [];
    private myService: INTTRADomainService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.myService = new INTTRADomainService();
        this.myService.GetINTTRACommunicationSettings().subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                this.EntityPM = myResponse.Result;

                this.IsResourcesReady = true;
            }
        });
    }

    get INTTRAProdFTPHost() { return this.EntityPM.INTTRAProdFTPHost; }
    set INTTRAProdFTPHost(value: string) {
        if (this.EntityPM.INTTRAProdFTPHost != value) {
            this.EntityPM.INTTRAProdFTPHost = value;
            this.SetUIProperties();
        }
    }

    get INTTRATestFTPHost() { return this.EntityPM.INTTRATestFTPHost; }
    set INTTRATestFTPHost(value: string) {
        if (this.EntityPM.INTTRATestFTPHost != value) {
            this.EntityPM.INTTRATestFTPHost = value;
            this.SetUIProperties();
        }
    }

    SetUIProperties() {
        var isProdFieldValid = true;
        var isTestFieldValid = true;
        var ProdFieldValidMessage = "";
        var TestFieldValidMessage = "";

        if (this.INTTRAProdFTPHost) {
            if (this.INTTRAProdFTPHost.length > 100) {
                isProdFieldValid = false;
                ProdFieldValidMessage = "INTTRA Prod FTP field must be less than 70 and more than 0";
            }
        }

        if (this.INTTRATestFTPHost) {
            if (this.INTTRATestFTPHost.length > 100) {
                isTestFieldValid = false;
                TestFieldValidMessage = "INTTRA Test FTP field must be less than 70 and more than 0";
            }
        }

        this.UIProperties.SetValidity("INTTRAProdFTPHost", this.ObjectTableName, isProdFieldValid, ProdFieldValidMessage);
        this.UIProperties.SetValidity("INTTRATestFTPHost", this.ObjectTableName, isTestFieldValid, TestFieldValidMessage);
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];

        if (this.INTTRAProdFTPHost) {
            if (this.INTTRAProdFTPHost.length > 100) {
                errors.push("INTTRA Prod FTP field must be less than 70 and more than 0");
            }
        }

        if (this.INTTRATestFTPHost) {
            if (this.INTTRATestFTPHost.length > 100) {
                errors.push("INTTRA Test FTP field must be less than 70 and more than 0");
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            this.CurrentSession.StartBusyIndicatorSaving();

            this.myService.UpdateINTTRACommunicationSettings(this.EntityPM).subscribe((myResponse: ServiceResponse) => {

                this.CurrentSession.StopBusyIndicator();

                if (myResponse.HasError) {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                }

                else {
                    this.CurrentSession.CloseCurrentWindow();
                }
            });
        }
    }
}
