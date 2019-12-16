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
import { GLAccountInterestPeriodPM } from '../../../EntityPMs/GLAccountInterestPeriodPM';
import { AppTool } from '../../../../Infrastructure/Tools';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { GLAccountValidator } from '../../../Validators/GLAccountValidator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { InterestBasesTypePM } from '../../../EntityPMs/InterestBasesTypePM';

@Component({
    moduleId: module.id,
    templateUrl: './GLAccountInterestComponent.html',
})

export class GLAccountInterestComponent extends BaseComponent{
    public EntityPM: GLAccountPM;
    public ObjectTableName: string = "GLAccount";
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
        //Resources
  
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
        if (this.EntityPM.ActiveForInterest) {
            this.UIProperties.SetEnabled("ActiveForInterestCreditInvoice", "GLAccount", true);
            this.UIProperties.SetEnabled("InterestCalculationStartDate", "GLAccount", true);
            this.UIProperties.SetEnabled("MinimumInterestInvoiceBilling", "GLAccount", true);
        }
        else {
            this.UIProperties.SetEnabled("ActiveForInterestCreditInvoice", "GLAccount", false);
            this.UIProperties.SetEnabled("InterestCalculationStartDate", "GLAccount", false);
            this.UIProperties.SetEnabled("MinimumInterestInvoiceBilling", "GLAccount", false);
        }
    }
    SetWindowArgs(args) {
        this.IsNew = args.IsNew;
    }
    //Grid Header Label
    public PeriodStartDateHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.PeriodStartDate");
    public StandardInterestRateBaseIdHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.StandardInterestRateBaseId");
    public StandardAdditionalInterestPercentageHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.StandardAddInterestPercent");
    public ExceptionalAdditionalInterestPercentageHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.ExceptionalAddInterestPercent");
    public ExceptionalInterestRateBaseIdHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.ExceptionalInterestRateBaseId");
    public CreditInterestRateBaseIdHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.CreditInterestRateBaseId");
    public CreditAdditionalInterestPercentageHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.CreditAddInterestPercent");
    public UpdatedByUserHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.UpdatedByUserId");
    public UpdateDateTimeHeader = TextCodeTranslator.Translate("GLAccountInterestPeriod.F.UpdateDateTime");

    OnRowEnded($event) {
        if (($event) == this.GLAccountInterestPeriodsList.Length) {
            this.AddLine();
        }
    }
    OnFocus() {
        if (this.GLAccountInterestPeriodsList.Length == 0) {
            this.AddLine();
        }
    }

    AddLine() {
        if (!this.ActiveForInterest) return;
        var errors = [];
        if (this.GLAccountInterestPeriodsList.Collection.length > 0) {
            // Validation
            var lastRow = this.GLAccountInterestPeriodsList.Collection[this.GLAccountInterestPeriodsList.Collection.length - 1];
            var ObjectTableName: string = "GLAccountInterestPeriod";
            Validator.TryValidateObject(lastRow, ObjectTableName, errors);
            this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            if (errors.length > 0) {
                return;
            }
        }
        // Adding New Line
        var gLAccountInterestPeriod: GLAccountInterestPeriodPM = new GLAccountInterestPeriodPM(this.EntityPM);
        gLAccountInterestPeriod.Tenant = this.EntityPM.Tenant;
        this.EntityPM.AddGLAccountInterestPeriod(gLAccountInterestPeriod);
        gLAccountInterestPeriod.LineNumber = this.GLAccountInterestPeriodsList.Collection.length > 0 ? (lastRow.LineNumber + 1) : 1;

        if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
            gLAccountInterestPeriod.GLAccountId = this.EntityPM.Id;
        }
        var line = new GLAccountInterestPeriodModel(gLAccountInterestPeriod, this);
        this.GLAccountInterestPeriodsList.Insert(line);
    }

    RemoveLine(line: GLAccountInterestPeriodModel) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.Areyousuredeleteline") + " " + line.LineNumber + " ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                  this.EntityPM.RemoveGLAccountInterestPeriod(line.EntityPM);
                this.GLAccountInterestPeriodsList.Remove(line);
                for (var i = 0; i < this.GLAccountInterestPeriodsList.Collection.length; i++) {
                    var oldItem = this.GLAccountInterestPeriodsList.Collection[i];
                    var updatedItem = this.GLAccountInterestPeriodsList.Collection[i];
                    updatedItem.LineNumber = i + 1;
                    this.GLAccountInterestPeriodsList.Update(oldItem, updatedItem);
                }
            }
        });

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
            list.push(new GLAccountInterestPeriodModel(item, this));
        });
        this.GLAccountInterestPeriodsList.InsertCollection(list);
    }

    private CheckFiveDigitBeforeComma(InterestRate: number): boolean {
        if ((InterestRate.toFixed()).length > 5) {
            return false;
        }
        return true;
    }

    get ActiveForInterest() {
            return this.EntityPM.ActiveForInterest;
      }
    set ActiveForInterest(newValue: boolean) {
            this.EntityPM.ActiveForInterest = newValue;
    }
    get ActiveForInterestCreditInvoice() {
        return this.EntityPM.ActiveForInterestCreditInvoice;
    }
    set ActiveForInterestCreditInvoice(newValue: boolean) {
            this.EntityPM.ActiveForInterestCreditInvoice = newValue;
    }
    get InterestCalculationStartDate() {
            return this.EntityPM.InterestCalculationStartDate;
      }
    set InterestCalculationStartDate(newValue: Date) {
            this.EntityPM.InterestCalculationStartDate = newValue;
     }
    get MinimumInterestInvoiceBilling() {
            return this.EntityPM.MinimumInterestInvoiceBilling;
    }
    set MinimumInterestInvoiceBilling(newValue: number) {
          if (!this.CheckFiveDigitBeforeComma(newValue))
              this.UIProperties.SetValidity("MinimumInterestInvoiceBilling", "GLAccount", false, "Number Of Digit Before Comma Must Be Five Or Less In Minimum Interest Invoice Billing Field");
          else
              this.UIProperties.SetValidity("MinimumInterestInvoiceBilling", "GLAccount", true, "Number Of Digit Before Comma Must Be Five Or Less In Minimum Interest Invoice Billing Field");

            this.EntityPM.MinimumInterestInvoiceBilling = newValue;
    }


    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }
}

export class GLAccountInterestPeriodModel extends BaseComponent {
    public DataContext: GLAccountInterestPeriodModel = this;
    public EntityPM: GLAccountInterestPeriodPM;
    public GLAccountPM: GLAccountPM;
    public ObjectTableName: string = "GLAccountInterestPeriod";

    constructor(entityPM: GLAccountInterestPeriodPM,  public fatherComponent) {
        super();
        this.EntityPM = entityPM;
        this.GLAccountPM = fatherComponent.EntityPM;
    }

    CheckTwoDigitBeforeComma(InterestRate: number): boolean {
        if ((InterestRate.toFixed()).length > 2) {
            return false;
        }
        return true;
    }


    get PeriodStartDate() { return this.EntityPM.PeriodStartDate; }
    set PeriodStartDate(newValue: Date) {
        if (this.EntityPM.PeriodStartDate != newValue) {
            this.EntityPM.PeriodStartDate = newValue;
        }
    }

    get GLAccountId() { return this.EntityPM.GLAccountId; }
    set GLAccountId(newValue: string) {
        if (this.EntityPM.GLAccountId != newValue) {
            this.EntityPM.GLAccountId = newValue;
        }
    }

    get StandardInterestRateBaseId() { return this.EntityPM.StandardInterestRateBaseId; }
    set StandardInterestRateBaseId(newValue: string) {
        if (this.EntityPM.StandardInterestRateBaseId != newValue) {
            this.EntityPM.StandardInterestRateBaseId = newValue;
        }
    }

    get StandardInterestRateBaseName() { return this.EntityPM.StandardInterestRateBaseName; }
    set StandardInterestRateBaseName(newValue: string) {
        if (this.EntityPM.StandardInterestRateBaseName != newValue) {
            this.EntityPM.StandardInterestRateBaseName = newValue;
        }
    }
    get StandardAddInterestPercent() { return this.EntityPM.StandardAddInterestPercent; }
    set StandardAddInterestPercent(newValue: number) {
        if (this.EntityPM.StandardAddInterestPercent != newValue) {
            if (!this.CheckTwoDigitBeforeComma(newValue))
                this.UIProperties.SetValidity("StandardAddInterestPercent", "GLAccountInterestPeriod", false, "Number Of Digit Before Comma Must Be Two Or Less In Standard Add Interest Percent");
            else
                this.UIProperties.SetValidity("StandardAddInterestPercent", "GLAccountInterestPeriod", true, "Number Of Digit Before Comma Must Be Two Or Less In Standard Add Interest Percent");

                this.EntityPM.StandardAddInterestPercent = newValue;
        }
    }
    private standardInterestRateBase: InterestBasesTypePM;
    get StandardInterestRateBase() { return this.standardInterestRateBase; }
    set StandardInterestRateBase(newValue: InterestBasesTypePM) {
        if (this.standardInterestRateBase != newValue) {
            this.standardInterestRateBase = newValue;
            this.StandardInterestRateBaseName = newValue.LocalName;
        }
    }

    get ExceptionalInterestRateBaseId() { return this.EntityPM.ExceptionalInterestRateBaseId; }
    set ExceptionalInterestRateBaseId(newValue: string) {
        if (this.EntityPM.ExceptionalInterestRateBaseId != newValue) {
            this.EntityPM.ExceptionalInterestRateBaseId = newValue;
        }
    }

    get ExceptionalInterestRateName() { return this.EntityPM.ExceptionalInterestRateName; }
    set ExceptionalInterestRateName(newValue: string) {
        if (this.EntityPM.ExceptionalInterestRateName != newValue) {
            this.EntityPM.ExceptionalInterestRateName = newValue;
        }
    }

    get ExceptionalAddInterestPercent() { return this.EntityPM.ExceptionalAddInterestPercent; }
    set ExceptionalAddInterestPercent(newValue: number) {
        if (this.EntityPM.ExceptionalAddInterestPercent != newValue) {
            if (!this.CheckTwoDigitBeforeComma(newValue))
                this.UIProperties.SetValidity("ExceptionalAddInterestPercent", "GLAccountInterestPeriod", false, "Number Of Digit Before Comma Must Be Two Or Less In Exceptional Add Interest Percent");
            else
                this.UIProperties.SetValidity("ExceptionalAddInterestPercent", "GLAccountInterestPeriod", true, "Number Of Digit Before Comma Must Be Two Or Less In Exceptional Add Interest Percent");

                this.EntityPM.ExceptionalAddInterestPercent = newValue;
        }
    }

    private exceptionalInterestRateBase: InterestBasesTypePM;
    get ExceptionalInterestRateBase() { return this.exceptionalInterestRateBase; }
    set ExceptionalInterestRateBase(newValue: InterestBasesTypePM) {
        if (this.exceptionalInterestRateBase != newValue) {
            this.exceptionalInterestRateBase = newValue;
            this.ExceptionalInterestRateName = newValue.LocalName;
        }
    }

    get CreditInterestRateBaseId() { return this.EntityPM.CreditInterestRateBaseId; }
    set CreditInterestRateBaseId(newValue: string) {
        if (this.EntityPM.CreditInterestRateBaseId != newValue) {
            this.EntityPM.CreditInterestRateBaseId = newValue;
        }
    }

    get CreditInterestRateBaseName() { return this.EntityPM.CreditInterestRateBaseName; }
    set CreditInterestRateBaseName(newValue: string) {
        if (this.EntityPM.CreditInterestRateBaseName != newValue) {
            this.EntityPM.CreditInterestRateBaseName = newValue;
        }
    }

    get CreditAddInterestPercent() { return this.EntityPM.CreditAddInterestPercent; }
    set CreditAddInterestPercent(newValue: number) {
        if (this.EntityPM.CreditAddInterestPercent != newValue) {
            if (!this.CheckTwoDigitBeforeComma(newValue))
                this.UIProperties.SetValidity("CreditAddInterestPercent", "GLAccountInterestPeriod", false, "Number Of Digit Before Comma Must Be Two Or Less In Credit Add Interest Percent");
            else
                this.UIProperties.SetValidity("CreditAddInterestPercent", "GLAccountInterestPeriod", true, "Number Of Digit Before Comma Must Be Two Or Less In Credit Add Interest Percent");
                this.EntityPM.CreditAddInterestPercent = newValue;
        }
    }

    private creditInterestRateBase: InterestBasesTypePM;
    get CreditInterestRateBase() { return this.creditInterestRateBase; }
    set CreditInterestRateBase(newValue: InterestBasesTypePM) {
        if (this.creditInterestRateBase != newValue) {
            this.creditInterestRateBase = newValue;
            this.CreditInterestRateBaseName = newValue.LocalName;
        }
    }

    get UpdateDateTime() { return this.EntityPM.UpdateDateTime; }
    set UpdateDateTime(newValue: Date) {
        if (this.EntityPM.UpdateDateTime != newValue) {
            this.EntityPM.UpdateDateTime = newValue;
        }
    }

    get LineNumber() { return this.EntityPM.LineNumber; }
    set LineNumber(newValue: number) {
        if (this.EntityPM.LineNumber != newValue) {
            this.EntityPM.LineNumber = newValue;
        }
    }

    get UpdatedByUserId() { return this.EntityPM.UpdatedByUserId; }
    set UpdatedByUserId(newValue: string) {
        if (this.EntityPM.UpdatedByUserId != newValue) {
            this.EntityPM.UpdatedByUserId = newValue;
        }
    }

    get UpdatedByUserName() { return this.EntityPM.UpdatedByUserName; }
    set UpdatedByUserName(newValue: string) {
        if (this.EntityPM.UpdatedByUserName != newValue) {
            this.EntityPM.UpdatedByUserName = newValue;
        }
    }

    get CloneMe() { return this.EntityPM.CloneMe; }
    get RejectChanges() { return this.EntityPM.RejectChanges; }


}
