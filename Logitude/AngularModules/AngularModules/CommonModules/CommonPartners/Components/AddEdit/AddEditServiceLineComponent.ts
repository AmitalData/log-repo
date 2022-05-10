import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { CarrierServiceLinePM } from '../../../../Common/EntityPMs/CarrierServiceLinePM';
import { ServiceLineItem } from '../EditTabs/ServiceLinesTabComponent';
import { CarrierServiceLinePMService } from '../../../../Common/Services/StandardPMs/CarrierServiceLinePMService';

@Component({
    templateUrl: './AddEditServiceLineComponent.html',
})

export class AddEditServiceLineComponent extends BaseComponent implements OnInit {
    public EntityPM: CarrierServiceLinePM;
    public ObjectTableName: string = "CarrierServiceLine";
    public DataContext: ServiceLineItem;
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

    SetDataContext(dataContext: ServiceLineItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNew = dataContext.IsNewEntity;

        this.Clone();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var service: CarrierServiceLinePMService = new CarrierServiceLinePMService();

            if (this.IsNew) {
                service.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveCompleted(myResponse);
                });
            }

            else {
                service.update(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
                    this.SaveCompleted(myResponse);
                });
            }
        }
    }

    private SaveCompleted(myResponse: ServiceResponse) {
        if (myResponse.HasError) {
            this.CurrentSession.StopBusyIndicator();
            this.ValidationErrorsList = myResponse.ErrorsArray;
        }

        else {
            this.DataContext.FatherComponent.LoadData();
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
        this.myCloner.AddField('Inactive');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

