import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { Validator } from '../../../../../Infrastructure/Validators/Validator';
import { TenantPM } from '../../../../../Common/EntityPMs/TenantPM';
import { ChequeCounterSerialPM } from 'Accounting/EntityPMs/ChequeCounterSerialPM';
import { BankAccountPM } from 'Accounting/EntityPMs/BankAccountPM';
import { BankAccountPMService } from 'Accounting/Services/StandardPMs/BankAccountPMService';
import { ConfirmWindow } from 'Controls/Windows/ConfirmWindow';

@Component({

    templateUrl: './ChequeCounterSerialComponent.html',
})

export class ChequeCounterSerialComponent extends BaseComponent implements OnInit {
    public EntityPM: BankAccountPM;
    public ObjectTableName: string = "ChequeCounterSerial";
    public DataContext: ChequeCounterSerialComponent = this;
    public ChequeCounterSerials: ObservableCollection;
    private CurrentSession = SessionLocator.SelectedSession;
    SeriesIdHeader: string = '';
    ChequeCounterBeginHeader: string = '';
    ChequeCounterEndHeader: string = '';
    InActiveHeader: string = '';
    public TenantPM: TenantPM;
    public isRTL: boolean = false;
    public ValidationErrorsList: string[] = [];
    SeriesId: number = null;
    chequeCounter: number;
    currentSerial: ChequeCounterSerialPM;
    myService: BankAccountPMService;
    constructor(public entityArgs: EntityArgs) {
        super();
        this.SeriesIdHeader = TextCodeTranslator.Translate("ChequeCounterSerial.CH.SeriesIdListLable");
        this.ChequeCounterBeginHeader = TextCodeTranslator.Translate("ChequeCounterSerial.CH.ChequeCounterBeginListLable");
        this.ChequeCounterEndHeader = TextCodeTranslator.Translate("ChequeCounterSerial.CH.ChequeCounterEndListLable");
        this.InActiveHeader = TextCodeTranslator.Translate("ChequeCounterSerial.CH.InactiveListLable");
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.TenantPM = SessionLocator.TenantPM;
        this.myService = new BankAccountPMService();
        this.ChequeCounterSerials = new ObservableCollection([]);
    }

    ngOnInit() {
    }

    SetUIProperties() {

    }

    SetWindowArgs(args: any) {
        this.EntityPM = args.EntityPM;
        this.SeriesId = this.EntityPM.ChequeCounterSeriesID;
        this.chequeCounter = this.EntityPM.ChequeCounter;

        this.SetUIProperties();
        this.BuildData();
        this.Listen();
    }

    private Listen() {
    }

    ValidateSerialsOverlap() {
        var res = false;
        this.ChequeCounterSerials.Collection.forEach(item => {
            var overlapSerials = this.ChequeCounterSerials.Collection.filter(s =>
                (item.ChequeCounterBegin >= s.ChequeCounterBegin && item.ChequeCounterBegin <= s.ChequeCounterEnd)
                ||
                (item.ChequeCounterEnd >= s.ChequeCounterBegin && item.ChequeCounterEnd <= s.ChequeCounterEnd));
            if (overlapSerials.length > 1) {
                res = true;
            }
        });
        return res;
    }

    AddLine() {
        var errors = [];
        if (this.ChequeCounterSerials.Collection.length > 0) {
            var lastRow = this.ChequeCounterSerials.Collection[this.ChequeCounterSerials.Collection.length - 1];
            Validator.TryValidateObject(lastRow, this.ObjectTableName, errors);
            lastRow.Validate(errors);
            this.ValidationErrorsList = errors;
            if (errors.length > 0) {
                return;
            }
        }

        // Adding New Line
        var chequeCounterSerial: ChequeCounterSerialPM = new ChequeCounterSerialPM(this.EntityPM);
        chequeCounterSerial.Tenant = this.EntityPM.Tenant;
        this.EntityPM.AddChequeCounterSerial(chequeCounterSerial);
        chequeCounterSerial.SeriesId = this.ChequeCounterSerials.Collection.length > 0 ? (lastRow.SeriesId + 1) : 1;

        if (!AppTool.IsNullOrEmpty(this.EntityPM)) {
            chequeCounterSerial.BankAccountId = this.EntityPM.Id;
        }
        var line = new ChequeCounterSerialItem(chequeCounterSerial, this);
        this.ChequeCounterSerials.Insert(line);
    }

    public BuildData() {
        this.ChequeCounterSerials.Clear();
        var list = [];
        this.EntityPM.ChequeCounterSerials.forEach(item => {
            list.push(new ChequeCounterSerialItem(item, this));
        });
        this.ChequeCounterSerials.InsertCollection(list);
        if (!AppTool.IsNullOrEmpty(this.SeriesId)) {
            this.currentSerial = this.ChequeCounterSerials.Collection.find(c => c.SeriesId == this.SeriesId);
        }
    }

    RemoveLine(line: ChequeCounterSerialItem) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show(TextCodeTranslator.Translate("Accounting.General.O.Areyousuredeleteline") + " ?");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.ChequeCounterSerials.Remove(line);
                this.EntityPM.RemoveChequeCounterSerial(line.EntityPM);
            }
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OnRowEnded($event) {
        if (($event) == this.ChequeCounterSerials.Length) {
            this.AddLine();
        }
    }

    OkButtonClicked() {
        var errors: string[] = [];
        this.ChequeCounterSerials.Collection.forEach(item => {
            Validator.TryValidateObject(item.EntityPM, this.ObjectTableName, errors);
            item.Validate(errors);
        });
        var hasOverlaps = this.ValidateSerialsOverlap();
        if (hasOverlaps) {
            errors.push(TextCodeTranslator.Translate("ChequeCounterSerial.O.SeriesOverlaps"));
        }
        if (errors.length == 0) {
            this.SubmitChanges();
        } else {
            this.ValidationErrorsList = errors;
        }
    }

    SubmitChanges() {
        var ChequeCounterSerials: ChequeCounterSerialPM[] = [];
        ChequeCounterSerials = this.EntityPM.ChequeCounterSerials;
        this.EntityPM.ChequeCounterSerials = null;
        this.EntityPM.ChequeCounterSerials = ChequeCounterSerials;
        this.myService.update(this.EntityPM).subscribe((myResult: any) => {
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

    ngOnDestroy() {
    }
}

export class ChequeCounterSerialItem extends BaseComponent {
    public DataContext: ChequeCounterSerialItem = this;
    public EntityPM: ChequeCounterSerialPM;
    public BankAccountPM: BankAccountPM;
    public ObjectTableName: string = "ChequeCounterSerial";
    errorChequeCounterBeginBigger = TextCodeTranslator.Translate("ChequeCounterSerial.O.ChequeCounterBeginBigger");
    errorChequeCounterBeginInvalid = TextCodeTranslator.Translate("ChequeCounterSerial.O.ChequeCounterBeginInvalid");
    errorChequeCounterEndInvalid = TextCodeTranslator.Translate("ChequeCounterSerial.O.ChequeCounterEndInvalid");
    errorChequeCounterEndSmaller = TextCodeTranslator.Translate("ChequeCounterSerial.O.ChequeCounterEndSmaller");

    constructor(entityPM: ChequeCounterSerialPM, public fatherComponent) {
        super();
        this.EntityPM = entityPM;
        this.BankAccountPM = fatherComponent.EntityPM;
        this.SetUIProperties();
    }

    SetUIProperties() {
        if (!AppTool.IsNullOrEmpty(this.BankAccountPM.ChequeCounterSeriesID) &&
            ((this.SeriesId < this.BankAccountPM.ChequeCounterSeriesID) ||
                (this.SeriesId == this.BankAccountPM.ChequeCounterSeriesID && this.BankAccountPM.ChequeCounter > this.ChequeCounterEnd)))
            this.UIProperties.SetEnabled("Inactive", this.ObjectTableName, false);
    }
    Validate(errors: string[]) {
        if (!this.CheckChequeCounterBeginSmaller(this.ChequeCounterBegin)) {
            errors.push(this.errorChequeCounterBeginBigger);
        }
        if (!this.CheckChequeCounterEndBigger(this.ChequeCounterEnd)) {
            errors.push(this.errorChequeCounterEndSmaller);
        }
        if (!this.CheckChequeCounterBeginValid(this.ChequeCounterBegin)) {
            errors.push(this.errorChequeCounterBeginInvalid);
        }
        if (!this.CheckChequeCounterEndValid(this.ChequeCounterEnd)) {
            errors.push(this.errorChequeCounterEndInvalid);
        }
    }

    CheckChequeCounterBeginSmaller(ChequeCounterBegin: number): boolean {
        if (ChequeCounterBegin != null && this.EntityPM.ChequeCounterEnd != null) {
            if (ChequeCounterBegin >= this.EntityPM.ChequeCounterEnd) {
                return false;
            }
            return true;
        }
        else {
            return true;
        }
    }
    CheckChequeCounterBeginValid(ChequeCounterBegin: number): boolean {
        if (ChequeCounterBegin != null) {
            if (ChequeCounterBegin < 1) {
                return false;
            }
            return true;
        }
        else {
            return true;
        }
    }
    CheckChequeCounterEndValid(ChequeCounterEndV: number): boolean {
        if (ChequeCounterEndV != null) {
            if (ChequeCounterEndV < 1) {
                return false;
            }
            return true;
        }
        else {
            return true;
        }
    }
    CheckChequeCounterEndBigger(ChequeCounterEnd: number): boolean {
        if (ChequeCounterEnd != null && this.EntityPM.ChequeCounterBegin != null) {
            if (ChequeCounterEnd <= this.EntityPM.ChequeCounterBegin) {
                return false;
            }
            return true;
        }
        else {
            return true;
        }
    }

    get SeriesId() { return this.EntityPM.SeriesId; }
    get BankAccountId() { return this.EntityPM.BankAccountId; }

    get ChequeCounterBegin() { return this.EntityPM.ChequeCounterBegin; }
    set ChequeCounterBegin(newValue: number) {
        if (this.EntityPM.ChequeCounterBegin != newValue) {
            this.EntityPM.ChequeCounterBegin = newValue;
        }
        if (!this.CheckChequeCounterBeginValid(newValue)) {
            this.UIProperties.SetValidity("ChequeCounterBegin", "ChequeCounterSerial", false, this.errorChequeCounterBeginInvalid);
        } else {
            if (!this.CheckChequeCounterBeginSmaller(newValue)) {
                this.UIProperties.SetValidity("ChequeCounterBegin", "ChequeCounterSerial", false, this.errorChequeCounterBeginBigger);
            } else {
                this.UIProperties.SetValidity("ChequeCounterBegin", "ChequeCounterSerial", true, '');
            }
        }
    }

    get ChequeCounterEnd() { return this.EntityPM.ChequeCounterEnd; }
    set ChequeCounterEnd(newValue: number) {
        if (this.EntityPM.ChequeCounterEnd != newValue) {
            this.EntityPM.ChequeCounterEnd = newValue;
        }
        if (!this.CheckChequeCounterEndValid(newValue)) {
            this.UIProperties.SetValidity("ChequeCounterEnd", "ChequeCounterSerial", false, this.errorChequeCounterEndInvalid);
        } else {
            if (!this.CheckChequeCounterEndBigger(newValue)) {
                this.UIProperties.SetValidity("ChequeCounterEnd", "ChequeCounterSerial", false, this.errorChequeCounterEndSmaller);
            } else {
                this.UIProperties.SetValidity("ChequeCounterEnd", "ChequeCounterSerial", true, '');
            }
        }
    }

    get Inactive() { return this.EntityPM.Inactive; }
    set Inactive(newValue: boolean) {
        if (this.EntityPM.Inactive != newValue) {
            this.EntityPM.Inactive = newValue;
        }
    }
}
