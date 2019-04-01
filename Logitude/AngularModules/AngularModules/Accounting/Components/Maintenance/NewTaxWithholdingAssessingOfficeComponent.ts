import {Component} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {TaxWithholdingAssessOfficePM} from '../../EntityPMs/TaxWithholdingAssessOfficePM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TaxWithholdingAssessOfficePMService} from '../../Services/StandardPMs/TaxWithholdingAssessOfficePMService';



@Component({
    selector: 'NewTaxWithholdingAssessingOfficeComponent',
    moduleId: module.id,
    templateUrl: './NewTaxWithholdingAssessingOfficeComponent.html',
})


export class NewTaxWithholdingAssessingOfficeComponent extends BaseComponent{

    public EntityPM: TaxWithholdingAssessOfficePM;
    public DataContext: any = this;
    public ObjectTableName: string = "TaxWithholdingAssessOffice";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: TaxWithholdingAssessOfficePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new TaxWithholdingAssessOfficePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new TaxWithholdingAssessOfficePMService();

    }

    // Properties
    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
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

    get InActive() { return this.EntityPM.Inactive; }
    set InActive(value: boolean) {
        if (this.EntityPM.Inactive != value) {
            this.EntityPM.Inactive = value;
        }
    }

    // Commands
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

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.CurrentSession.CloseCurrentWindowEmit("ok");
            }

            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

}
