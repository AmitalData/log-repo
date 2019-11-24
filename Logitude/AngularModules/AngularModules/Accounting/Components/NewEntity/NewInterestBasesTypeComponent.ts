import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';
import { InterestBasesTypePMService } from '../../Services/StandardPMs/InterestBasesTypePMService';

@Component({
    selector: 'NewInterestBasesTypeComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewInterestBasesTypeComponent.html',
})

export class NewInterestBasesTypeComponent extends BaseComponent{
    public EntityPM: InterestBasesTypePM;
    public DataContext: NewInterestBasesTypeComponent = this;
    public ObjectTableName: string = "InterestBasesType";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: InterestBasesTypePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new InterestBasesTypePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new InterestBasesTypePMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
    }

    // Properties
    get Inactive() { return this.EntityPM.InActive == null ? false : this.EntityPM.InActive; }
    set Inactive(value: boolean) {
        if (this.Inactive != value) {
            this.EntityPM.InActive = value;
        }
    }

    get Code() { return this.EntityPM.Code; }
    set Code(value: string) {
        if (this.EntityPM.Code != value) {
            this.EntityPM.Code = value;
        }
    }

    get LocalName() { return this.EntityPM.LocalName; }
    set LocalName(value: string) {
        if (this.EntityPM.LocalName != value) {
            this.EntityPM.LocalName = value;
        }
    }

    get EnglishName() { return this.EntityPM.EnglishName; }
    set EnglishName(value: string) {
        if (this.EntityPM.EnglishName != value) {
            this.EntityPM.EnglishName = value;
        }
    }

    get Description() { return this.EntityPM.Description; }
    set Description(value: string) {
        if (this.EntityPM.Description != value) {
            this.EntityPM.Description = value;
        }
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges() {
        this.myService.insert(this.EntityPM).subscribe(myResult => {
            var iServiceResponse: ServiceResponse = myResult;
            if (!iServiceResponse.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
            else {
                this.ValidationErrorsList = iServiceResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    SetUIProperties() {
    }
    
    SelectDefaultValues() {
        this.EntityPM.InActive = false;
    }
     
}
