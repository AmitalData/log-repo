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
    templateUrl: './AddEditAirlineAreaComponent.html',
})

export class AddEditAirlineAreaComponent extends BaseComponent {
    public EntityPM: AirlinePM;
    public ObjectTableName: string="AirlineAreasPort";
    public DataContext: AddEditAirlineAreaComponent = this;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.EntityPM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];

        this.IsResourcesReady = true;
    }


   

    private myService;
    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    private Close(event: any) {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

  


}
