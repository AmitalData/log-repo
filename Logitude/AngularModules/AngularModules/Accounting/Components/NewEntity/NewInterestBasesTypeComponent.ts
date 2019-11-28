import {Component, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {EntityListService} from '../../../Infrastructure/Services/EntityListService';
import { InterestBasesTypePM } from '../../EntityPMs/InterestBasesTypePM';
import { InterestBasesTypePMService } from '../../Services/StandardPMs/InterestBasesTypePMService';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { InterestBasesPeriodItem } from '../EditTabs/Interest/DetailsTab/InterestBasesTypeDetailsTabComponent';
import { InterestBasesPeriodPM } from '../../EntityPMs/InterestBasesPeriodPM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';

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
    public InterestBasesPeriodsList: ObservableCollection;
    public isRTL: boolean = false;
    myService: InterestBasesTypePMService;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityListService: EntityListService) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.TenantPM = SessionLocator.TenantPM;
        this.EntityPM = new InterestBasesTypePM();
        this.InterestBasesPeriodsList = new ObservableCollection([]);
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.myService = new InterestBasesTypePMService();
        this.SetUIProperties();
        this.SelectDefaultValues();
    }

    //Grid Header Label
    public InterestBaseStartDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestBaseStartDateListLable");
    public UpdatedByUserIdHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdatedByUserIdListLable");
    public UpdateDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdateDateListLable");
    public InterestRateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestRateListLable");
    //Add Edit Interest Bases Period Title
    public EditInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.O.Edit");
    public AddInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.B.Add");

    // Properties
    get Inactive() { return this.EntityPM.InActive == null ? false : this.EntityPM.InActive; }
    set Inactive(value: boolean) {
        if (this.Inactive != value) {
            this.EntityPM.InActive = value;
        }
    }

    public BuildData() {
        this.InterestBasesPeriodsList.Clear();
        var list = [];
        this.EntityPM.InterestBasesPeriods.forEach(item => {
            list.push(new InterestBasesPeriodItem(item, true, this));
        });
        this.InterestBasesPeriodsList.InsertCollection(list);
    }

    AddPeriodClicked() {
        var itemPM = new InterestBasesPeriodPM(null);
        itemPM.Tenant = SessionLocator.Tenant;
        itemPM.InterestBaseTypeId = this.EntityPM.Id;
        itemPM.LineNumber = this.EntityPM.InterestBasesPeriods.length + 1;
        var itemComponent = new InterestBasesPeriodItem(itemPM, true, this);
        this.LogWindowShow(this.AddInterestBasesPeriod, itemComponent);
    }

    EditPeriodClicked(itemComponent: InterestBasesPeriodItem) {
        itemComponent.IsNewEntity = false;
        this.LogWindowShow(this.EditInterestBasesPeriod, itemComponent);
    }

    LogWindowShow(title: string, itemComponent) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        var myPath = "./Accounting/Components/Packages/EditTabs/Interest/DetailsTab/AddEditInterestBasesPeriod/AddEditInterestBasesPeriodComponent";
        logWindow.Width = 600;
        logWindow.Height = 250;
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
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
        this.EntityPM.InterestBasesPeriods = null;
        this.myService.insert(this.EntityPM).subscribe(myResult => {
            var iServiceResponse: ServiceResponse = myResult;
            if (!iServiceResponse.HasError) {
                if (this.InterestBasesPeriodsList.Collection != null && this.InterestBasesPeriodsList.Collection.length > 0) {
                    this.UpdateEntityPM();
                }
                else
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
            }
            else {
                this.ValidationErrorsList = iServiceResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }

    UpdateEntityPM() {
        //this.EntityPM.InterestBasesPeriods = this.InterestBasesPeriodsList.Collection;
        this.myService.update(this.EntityPM).subscribe(myResult => {
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
