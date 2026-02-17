import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ChartOfAccountPM} from '../../EntityPMs/ChartOfAccountPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {ChartOfAccountPMService} from '../../Services/StandardPMs/ChartOfAccountPMService';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';

@Component({
    selector: 'NewChartOfAccountComponent',
    moduleId: module.id,
    providers: [EntityListService],
    templateUrl: './NewChartOfAccountComponent.html',
})

export class NewChartOfAccountComponent extends BaseComponent{
    public EntityPM: ChartOfAccountPM;
    public DataContext: NewChartOfAccountComponent = this;
    public ObjectTableName: string = "ChartOfAccount";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    myService: ChartOfAccountPMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef, public entityListService: EntityListService) {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new ChartOfAccountPM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new ChartOfAccountPMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
        this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);

    }

    // Properties
    get Inactive() { return this.EntityPM.Inactive == null ? false : this.EntityPM.Inactive; }
    set Inactive(value: boolean) {
        if (this.Inactive != value) {
            this.EntityPM.Inactive = value;
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

    get TypeCode() { return this.EntityPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.EntityPM.TypeCode != value) {
            this.EntityPM.TypeCode = value;
            if (value != null) {
                this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, true);
            } else {
                this.ParentId = null;
                this.UIProperties.SetEnabled("ParentId", this.ObjectTableName, false);
            }
        }
    }

    get ParentId() { return this.EntityPM.ParentId; }
    set ParentId(value: string) {
        if (this.EntityPM.ParentId != value) {
            this.EntityPM.ParentId = value;
        }
    }
    

    OkButtonClicked() {
        var errors: string[] = [];

        //if (this.IsMultiCurrency) {
        //    if (this.ReconcileMethodCode != '0') {
        //        errors.push("The reconcile method for multi currency GLAaccount must be local currency"); // need a textcode to enable translations to hebrew
        //    }
        //}

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

    SetUIProperties() {
        //this.UIProperties.SetEnabled("ChartOfAccountsId", this.ObjectTableName, false);
        //this.UIProperties.SetEnabled("CurrencyId", this.ObjectTableName, true);
        //this.UIProperties.SetRequired("CurrencyId", this.ObjectTableName, true);
    }
    
    SelectDefaultValues() {
        this.EntityPM.Inactive = false;
        
    }
     
}
