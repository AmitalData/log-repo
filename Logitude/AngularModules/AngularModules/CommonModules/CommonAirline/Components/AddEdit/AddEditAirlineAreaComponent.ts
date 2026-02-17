import { Component, Output, OnInit} from '@angular/core';
import { AirlinePM } from '../../../../Common/EntityPMs/AirlinePM';
import { AirlineAreaPM } from '../../../../Common/EntityPMs/AirlineAreaPM';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { AreaItemClass } from '../EditTabs/AreasTabComponent';
import { AirlineAreaPMService } from '../../../../Common/Services/StandardPMs/AirlineAreaPMService';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditAirlineAreaComponent.html',
})

export class AddEditAirlineAreaComponent extends BaseComponent implements OnInit {
    public EntityPM: AirlineAreaPM;
    public ObjectTableName: string="AirlineArea";
    public DataContext: AreaItemClass;
    public IsNew: boolean;   
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    ngOnInit() {
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
    }

    SetDataContext(dataContext: AreaItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNew = dataContext.IsNewEntity;
        
        this.Clone();
    }
    
    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (AppTool.IsNullOrEmpty(this.EntityPM.Name)) {
            this.ValidationErrorsList.push("Name is required");
        }

        if (this.EntityPM.AirlineAreasPorts.filter(p => p.ChangeSetOp != "3")[0] == null) {
            this.ValidationErrorsList.push("At Least one port is required");
        }       
      
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var service: AirlineAreaPMService = new AirlineAreaPMService();
            
            if (this.IsNew) {
                service.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveAreasCompleted(myResponse);                                       
                });
            }

            else {
                service.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveAreasCompleted(myResponse);
                });
            }            
        }       
    }

    private SaveAreasCompleted(myResponse: ServiceResponse) {
        if (myResponse.HasError) {
            this.CurrentSession.StopBusyIndicator();
            this.ValidationErrorsList = myResponse.ErrorsArray;
        }

        else {
            this.DataContext.fatherComponent.LoadData();
            this.CurrentSession.StopBusyIndicator();
            this.CurrentSession.CloseCurrentWindow();
        } 
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('Description');
        this.myCloner.AddField('Name');        
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.EntityPM.AirlineAreasPorts);
        this.EntityPM.AirlineAreasPorts.forEach(p => {
            this.myCloner.AddEntity(p);
        });
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
