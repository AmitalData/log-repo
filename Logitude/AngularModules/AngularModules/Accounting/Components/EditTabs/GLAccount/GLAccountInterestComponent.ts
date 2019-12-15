import {Component}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { GLAccountPM } from '../../../EntityPMs/GLAccountPM';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TenantPM } from '../../../../Common/EntityPMs/TenantPM';
import { GLAccountPMService } from '../../../Services/StandardPMs/GLAccountPMService';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { GLAccountInterestPeriodPM } from '../../../EntityPMs/GLAccountIinterestPeriodPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    moduleId: module.id,
    templateUrl: './GLAccountInterestComponent.html',
})

export class GLAccountInterestComponent extends BaseComponent{
    public EntityPM: GLAccountPM;
    public ObjectTableName: string = "GLAccountInterestPeriod";
    public DataContext: GLAccountInterestComponent = this;
    public GLAccountInterestPeriodsList: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    public TenantPM: TenantPM;
    public IsNew: boolean = false;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    myService: GLAccountPMService;
    constructor(public entityArgs: EntityArgs) {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        this.TenantPM = SessionLocator.TenantPM;
        if (!this.EntityPM) {
            this.EntityPM = new GLAccountPM();
            this.EntityPM.Tenant = this.TenantPM.Id;
        }
        this.myService = new GLAccountPMService();
        this.GLAccountInterestPeriodsList = new ObservableCollection([]);
        this.EntityPM.OldEntityPM = this.EntityPM;
        this.BuildData();
        this.SetUIProperties();
        this.Listen();
    }
    ngOnInit() {
    }
    SetUIProperties() {
        //if (this.EntityPM.InActive) {
        //    this.UIProperties.SetEnabled("Code", "InterestBasesType", false);
        //    this.UIProperties.SetEnabled("EnglishName", "InterestBasesType", false);
        //    this.UIProperties.SetEnabled("LocalName", "InterestBasesType", false);
        //    this.UIProperties.SetEnabled("Description", "InterestBasesType", false);
        //}
        //else {
        //    this.UIProperties.SetEnabled("Code", "InterestBasesType", true);
        //    this.UIProperties.SetEnabled("EnglishName", "InterestBasesType", true);
        //    this.UIProperties.SetEnabled("LocalName", "InterestBasesType", true);
        //    this.UIProperties.SetEnabled("Description", "InterestBasesType", true);
        //}
    }
    SetWindowArgs(args) {
        this.IsNew = args.IsNew;
    }
    //Grid Header Label
    public InterestBaseStartDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestBaseStartDateListLable");
    public UpdatedByUserIdHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdatedByUserIdListLable");
    public UpdateDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdateDateListLable");
    public InterestRateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestRateListLable");
    public InterestBaseStartDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestBaseStartDateListLable");
    public UpdatedByUserIdHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdatedByUserIdListLable");
    public UpdateDateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.UpdateDateListLable");
    public InterestRateHeader = TextCodeTranslator.Translate("InterestBasesPeriod.CH.InterestRateListLable");

    //Add Edit Interest Bases Period Title
    public EditInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.O.Edit");
    public AddInterestBasesPeriod = TextCodeTranslator.Translate("Accounting.General.B.Add");

    AddPeriodClicked() {
        //if (!this.EntityPM.InActive) {
        //    var itemPM = new InterestBasesPeriodPM(null);
        //    itemPM.Tenant = SessionLocator.Tenant;
        //    itemPM.InterestBaseTypeId = this.EntityPM.Id;
        //    itemPM.LineNumber = this.EntityPM.InterestBasesPeriods.length + 1;
        //    var itemComponent = new InterestBasesPeriodItem(itemPM, true, this);
        //    this.LogWindowShow(this.AddInterestBasesPeriod, itemComponent);
        //}
    }

    EditPeriodClicked(itemComponent: GLAccountInterestPeriod) {
        //if (!this.EntityPM.InActive) {
        //    itemComponent.IsNewEntity = false;
        //    this.LogWindowShow(this.EditInterestBasesPeriod, itemComponent);
        //}
    }

    LogWindowShow(title: string, itemComponent) {
        var logWindow = new LogitudeWindow();
        logWindow.Title = title;
        var myPath = "./Accounting/Components/Packages/EditTabs/Interest/DetailsTab/AddEditInterestBasesPeriod/AddEditInterestBasesPeriodComponent";
        logWindow.Width = 400;
        logWindow.Height = 200;
        logWindow.DataContext = itemComponent;
        logWindow.Show(myPath);
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.BuildData();
                    this.SetUIProperties();
                }
            });
        }
    }

    public BuildData() {
        this.GLAccountInterestPeriodsList.Clear();
        var list = [];
        this.EntityPM.GLAccountInterestPeriods.forEach(item => {
            list.push(new GLAccountInterestPeriod(item, true, this));
        });
        this.GLAccountInterestPeriodsList.InsertCollection(list);
    }


    get ActiveForInterest() {
        if (this.EntityPM != null) {
            return this.EntityPM.ActiveForInterest;
        }
        else
            return null;
    }
    set ActiveForInterest(newValue: boolean) {
        if (this.EntityPM.ActiveForInterest != newValue) {
            this.EntityPM.ActiveForInterest = newValue;
        }
    }
    get ActiveForInterestCreditInvoice() {
        if (this.ActiveForInterestCreditInvoice != null) {
            return this.EntityPM.ActiveForInterestCreditInvoice;
        }
        else
            return null;
    }
    set ActiveForInterestCreditInvoice(newValue: boolean) {
        if (this.EntityPM.ActiveForInterestCreditInvoice != newValue) {
            this.EntityPM.ActiveForInterestCreditInvoice = newValue;
        }
    }
    get InterestCalculationStartDate() {
        if (this.EntityPM != null) {
            return this.EntityPM.InterestCalculationStartDate;
        }
        else
            return null;
    }
    set InterestCalculationStartDate(newValue: Date) {
        if (this.EntityPM.InterestCalculationStartDate != newValue) {
            this.EntityPM.InterestCalculationStartDate = newValue;
        }
    }
    get MinimumInterestInvoiceBilling() {
        if (this.EntityPM != null) {
            return this.EntityPM.MinimumInterestInvoiceBilling;
        }
        else
            return null;
    }
    set MinimumInterestInvoiceBilling(newValue: number) {
        if (this.EntityPM.MinimumInterestInvoiceBilling != newValue) {
            this.EntityPM.MinimumInterestInvoiceBilling = newValue;
        }
    }


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}

export class GLAccountInterestPeriod extends BaseComponent {
    public DataContext: GLAccountInterestPeriod = this;
    public EntityPM: GLAccountInterestPeriodPM;
    public GLAccountPM: GLAccountPM;
    public ObjectTableName: string = "GLAccountInterestPeriod";
    public IsNewEntity: boolean = false;

    constructor(entityPM: GLAccountInterestPeriodPM, isNew: boolean, public fatherComponent) {
        super();
        this.EntityPM = entityPM;
        this.GLAccountPM = fatherComponent.EntityPM;
        this.IsNewEntity = isNew;
    }

    CheckInterestRateValid(InterestRate: number): boolean {
        if ((InterestRate.toFixed()).length > 2) {
            return false;
        }
        return true;
    }
    CheckInterestBaseStartDateExist(InterestBaseStartDate: Date, CreateDate: Date): boolean {
        for (let i = 0; i < this.DataContext.fatherComponent.InterestBasesPeriodsList.Length; i++) {
            if (new Date(this.DataContext.fatherComponent.InterestBasesPeriodsList.Collection[i].InterestBaseStartDate).getTime() === new Date(InterestBaseStartDate).getTime()) {
                var idont;
                if (!this.IsNewEntity) {
                    if (new Date(this.DataContext.fatherComponent.InterestBasesPeriodsList.Collection[i].CreateDate).getTime() != new Date(CreateDate).getTime()) {
                        return false;
                    }
                }
                else {
                    return false;
                }
            }
        }
        return true;
    }

    get PeriodStartDate() { return this.EntityPM.PeriodStartDate; }
    set PeriodStartDate(newValue: Date) {
        if (this.EntityPM.PeriodStartDate != newValue) {
            this.EntityPM.PeriodStartDate = newValue;
        }
    }

    get StandardInterestRateBaseId() { return this.EntityPM.StandardInterestRateBaseId; }
    set StandardInterestRateBaseId(newValue: string) {
        if (this.EntityPM.StandardInterestRateBaseId != newValue) {
            this.EntityPM.StandardInterestRateBaseId = newValue;
        }
    }

    get StandardAddInterestPercent() { return this.EntityPM.StandardAddInterestPercent; }
    set StandardAddInterestPercent(newValue: number) {
        if (this.EntityPM.StandardAddInterestPercent != newValue) {
            this.EntityPM.StandardAddInterestPercent = newValue;
        }
    }

    get ExceptionalInterestRateBaseId() { return this.EntityPM.ExceptionalInterestRateBaseId; }
    set ExceptionalInterestRateBaseId(newValue: string) {
        if (this.EntityPM.ExceptionalInterestRateBaseId != newValue) {
            this.EntityPM.ExceptionalInterestRateBaseId = newValue;
        }
    }

    get ExceptionalAddInterestPercent() { return this.EntityPM.ExceptionalAddInterestPercent; }
    set ExceptionalAddInterestPercent(newValue: number) {
        if (this.EntityPM.ExceptionalAddInterestPercent != newValue) {
            this.EntityPM.ExceptionalAddInterestPercent = newValue;
        }
    }

    get CreditInterestRateBaseId() { return this.EntityPM.CreditInterestRateBaseId; }
    set CreditInterestRateBaseId(newValue: string) {
        if (this.EntityPM.CreditInterestRateBaseId != newValue) {
            this.EntityPM.CreditInterestRateBaseId = newValue;
        }
    }

    get CreditAddInterestPercent() { return this.EntityPM.CreditAddInterestPercent; }
    set CreditAddInterestPercent(newValue: number) {
        if (this.EntityPM.CreditAddInterestPercent != newValue) {
            this.EntityPM.CreditAddInterestPercent = newValue;
        }
    }

    get UpdateDateTime() { return this.EntityPM.UpdateDateTime; }
    set UpdateDateTime(newValue: Date) {
        if (this.EntityPM.UpdateDateTime != newValue) {
            this.EntityPM.UpdateDateTime = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get CloneMe() { return this.EntityPM.CloneMe; }
    get RejectChanges() { return this.EntityPM.RejectChanges; }


}
