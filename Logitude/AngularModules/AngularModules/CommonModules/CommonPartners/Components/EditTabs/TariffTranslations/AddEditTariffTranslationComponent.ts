import { Component, OnInit } from '@angular/core';
import { TariffCarrierTranslationPM } from '../../../../../Common/EntityPMs/TariffCarrierTranslationPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { Cloner } from '../../../../../Infrastructure/Utilities/Cloner';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { TariffCarrierTranslationPMService } from '../../../../../Common/Services/StandardPMs/TariffCarrierTranslationPMService';
import { TranslationItemClass } from './TariffTranslationsTabComponent';

@Component({
    
    templateUrl: './AddEditTariffTranslationComponent.html',
})

export class AddEditTariffTranslationComponent extends BaseComponent implements OnInit {
    public EntityPM: TariffCarrierTranslationPM;
    public ObjectTableName: string = "TariffCarrierTranslation";
    public DataContext: TranslationItemClass;
    public IsNew: boolean;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    public PortDependencyFilterValue: string;
    constructor() {
        super();
    }

    ngOnInit() {
        if (this.DataContext != null) {
            this.DataContext.SetUIProperties();
        }
    }

    SetDataContext(dataContext: TranslationItemClass) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.IsNew = dataContext.IsNewEntity;
        this.PortDependencyFilterValue = dataContext.fatherComponent.TransportModeCode;

        this.Clone();
    }

    SaveButtonClicked() {
        this.ValidationErrorsList = [];

        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.ValidationErrorsList);
        
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicatorSaving();

            var service: TariffCarrierTranslationPMService = new TariffCarrierTranslationPMService();

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
        this.myCloner.AddField('PartnerCode');
        this.myCloner.AddField('PortId');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

