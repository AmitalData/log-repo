import {Component, Output, EventEmitter} from '@angular/core';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';
import {AWBSpecialHandlingCodeExtendedPMService} from '../../../../Shipment/Services/ExtendedPMs/AWBSpecialHandlingCodeExtendedPMService';
import {IATACodeExtendedPMService} from '../../../../Infrastructure/Services/ExtendedPMs/IATACodeExtendedPMService';
import {BookingProductExtendedPMService} from '../../../../Booking/Services/ExtendedPMs/BookingProductExtendedPMService';
import {CommodityPMService} from '../../../../Common/Services/StandardPMs/CommodityPMService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirlineAdaptationItemComponent.html',
})

export class AddEditAirlineAdaptationItemComponent extends BaseComponent {
    public AirlinePM: AirlinePM;
    public EntityPM;
    public ObjectTableName: string;
    public DataContext: AddEditAirlineAdaptationItemComponent = this;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.AirlinePM = windowArgs['AirlinePM'];
        this.EntityPM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        this.ObjectTableName = windowArgs['ObjectTableName'];
        this.SetUIProperties();
        this.Clone();

        this.IsResourcesReady = true;
    }

    SetUIProperties() {
        this.UIProperties.SetVisibility("InActive", this.ObjectTableName, !this.IsNew);
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get InActive() { return this.EntityPM.InActive; }
    set InActive(value: boolean) {
        if (this.EntityPM.InActive != value) {
            this.EntityPM.InActive = value;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myService;
    OkButtonClicked() {
        var errors: string[] = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.SetService();

            if (this.IsNew) {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.insert(this.EntityPM).subscribe(Result => {

                    var mm: ServiceResponse = Result;
                    if (!mm.HasError) {
                        CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        CachedDataManager.RefreshCompleted.subscribe(($event: any) => this.Close($event));
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }

            else {
                this.CurrentSession.StartBusyIndicatorSaving();
                this.myService.update(this.EntityPM).subscribe(Result => {

                    var mm: ServiceResponse = Result;
                    if (!mm.HasError) {
                        CachedDataManager.RefreshTableData(this.ObjectTableName, true);
                        CachedDataManager.RefreshCompleted.subscribe(($event: any) => this.Close($event));
                    }

                    else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                });
            }
        }
    }

    private Close(event: any) {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

    private SetService() {
        switch (this.ObjectTableName) {
            case "AWBSpecialHandlingCode": {
                this.myService = new AWBSpecialHandlingCodeExtendedPMService();
                break;
            }

            case "Commodity": {
                this.myService = new CommodityPMService();
                break;
            }

            case "IATACode": {
                this.myService = new IATACodeExtendedPMService();
                break;
            }

            case "BookingProduct": {
                this.myService = new BookingProductExtendedPMService();
                break;
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Name');
        this.myCloner.AddField('Code');
        this.myCloner.AddField('InActive');
        this.myCloner.AddEntity(this.EntityPM);
    }

    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
