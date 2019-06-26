import {Component, Output, EventEmitter} from '@angular/core';
import { AirlinePM } from '../../../../Common/EntityPMs/AirlinePM';
import { AirlineAreaPM } from '../../../../Common/EntityPMs/AirlineAreaPM';

import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CachedDataManager } from '../../../../Infrastructure/Utilities/CachedDataManager';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirlineAreaComponent.html',
})

export class AddEditAirlineAreaComponent extends BaseComponent {
    public EntityPM: AirlineAreaPM;
    public AirlinePM: AirlinePM;

    public ObjectTableName: string="AirlineArea";
    public DataContext: AddEditAirlineAreaComponent = this;
    public IsNew: boolean;
    public ItemList = [];
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    

    constructor() {
        super();
    }

    public get Name() { return this.EntityPM.Name; }
    public set Name(value: string) { this.EntityPM.Name = value; }


    public get Description() { return this.EntityPM.Description; }
    public set Description(value: string) { this.EntityPM.Description = value; }
    
    public IsResourcesReady: boolean = false;
    SetWindowArgs(windowArgs: any) {
        this.AirlinePM = windowArgs['EntityPM'];
        this.IsNew = windowArgs['IsNew'];
        if (this.IsNew) {
            this.EntityPM = new AirlineAreaPM(this.AirlinePM);
        }
        this.IsResourcesReady = true;
    }


   

    CloseButtonClicked() {
        if (this.IsNew) {
            this.EntityPM.CreatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
            this.EntityPM.UpdatedByUserName = SessionInfo.LoggedUserPM.EnglishName;
            this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();

            this.AirlinePM.AddAirlineAreaPM(this.EntityPM);
        }
        else {

        }
        this.CurrentSession.CloseCurrentWindow();
    }

    private Close(event: any) {
        this.CurrentSession.StopBusyIndicator();
        this.CurrentSession.CloseCurrentWindowEmit("ok");
    }

  


}
