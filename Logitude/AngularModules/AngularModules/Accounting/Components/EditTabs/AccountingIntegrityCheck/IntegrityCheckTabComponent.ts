import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { AccountingIntegrityCheckPM } from '../../../EntityPMs/AccountingIntegrityCheckPM';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { EntityListService } from '../../../../Infrastructure/Services/EntityListService';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { AccountingEntegrityCheckExtendedPMService } from '../../../Services/ExtendedPMs/AccountingEntegrityCheckExtendedPMService';
import { AccountingIntegrityCheckPMService } from '../../../Services/StandardPMs/AccountingIntegrityCheckPMService';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import * as XLSX from 'xlsx'; 

@Component({

    templateUrl: './IntegrityCheckTabComponent.html',
})

export class IntegrityCheckTabComponent extends BaseComponent implements OnInit {

    public entityPM: AccountingIntegrityCheckPM = null;
    public ObjectTableName = "AccountingIntegrityCheck";
    public DataContext = this;
    public isRTL: boolean = false;
    public showLocals: boolean = false;
    AccountingEntegrityCheckExtendedPMService: AccountingEntegrityCheckExtendedPMService = new AccountingEntegrityCheckExtendedPMService();
    public _parameters: IntegrityCheckParameters = new IntegrityCheckParameters();
    AccountingIntegrityCheckPMService: AccountingIntegrityCheckPMService = new AccountingIntegrityCheckPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    HasException: boolean = false;
    ShouldFix: boolean = false;
    Fixing: boolean = false;
    constructor(private entityArgs: EntityArgs, private CD: ChangeDetectorRef) {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.entityPM = entityArgs.EntityPM;
        this.HasException = this.entityPM.HasException;
        this.ShouldFix = this.entityPM.ShouldFix;
        // this.encodeParameters();
        // this.decodeParameters();
        if (this.entityPM.StatusCode == "2") this.Fixing = true;
        this.SetUIProperty();

    }
    ngOnInit() {
    }

    ReloadScreen() {
    }

    cleanAndCalculate(obj, propertiesToRemove = null, currentlyMaxLengths) {
        var res = {
            obj: null,
            propertiesMaxLength: currentlyMaxLengths
        };
        for (var propName in obj) {
            if (obj[propName] === null || obj[propName] === undefined || propertiesToRemove.includes(propName)) {
                delete obj[propName];
            } else {
                var headerLen = propName.length;
                var propLen = obj[propName].toString().length;
                if (res.propertiesMaxLength.has(propName)) {
                    if (res.propertiesMaxLength.get(propName) < propLen) {
                        res.propertiesMaxLength.set(propName, propLen);
                    }
                } else {
                    var maxLen = Math.max(headerLen, propLen);
                    res.propertiesMaxLength.set(propName, maxLen);
                }
            }
        }
        res.obj = obj;
        return res
    }

    private getFinalObjectForExcelArray(originalObj, originalObjName, cleanPropertiesArray) {
        let propertiesMaxLength = new Map<string, number>();
        let wscols = [];
        let newObj = originalObj.map(o => {
            let calc = this.cleanAndCalculate(o, cleanPropertiesArray, propertiesMaxLength);
            propertiesMaxLength = calc.propertiesMaxLength;
            return calc.obj;
        });
        if (newObj && newObj.length > 0) {
            for (let value of propertiesMaxLength.values()) {
                wscols.push({ wch: value });
            }
            return {
                array: newObj,
                name: originalObjName,
                wscols: wscols
            };
        } else {
            return null;
        }
    }

    public Export2ExcelClicked() {
        this.AccountingIntegrityCheckPMService.getAccountingIntegrityResultByIdAndTenant(this.entityPM.Id, this.entityPM.Tenant).subscribe((myResponse: ServiceResponse) => {
            let resArr = [];
            let journalLineToLedgerResult = this.getFinalObjectForExcelArray(myResponse.Result.JournalLineToLedgerResult, 'JournalLineToLedgerResult', ['$id']);
            if (journalLineToLedgerResult != null) {
                resArr.push(journalLineToLedgerResult);
            }

            let ledgerToMounthTotalResult = this.getFinalObjectForExcelArray(myResponse.Result.LedgerToMounthTotalResult, 'LedgerToMounthTotalResult', ['$id']);
            if (ledgerToMounthTotalResult != null) {
                resArr.push(ledgerToMounthTotalResult);
            }

            let balanceInLocalCurrencyResult = this.getFinalObjectForExcelArray(myResponse.Result.BalanceInLocalCurrencyResult, 'BalanceInLocalCurrencyResult', ['$id']);
            if (balanceInLocalCurrencyResult != null) {
                resArr.push(balanceInLocalCurrencyResult);
            }

            let dueLocalBalance = this.getFinalObjectForExcelArray(myResponse.Result.DueLocalBalance, 'DueLocalBalance', ['$id']);
            if (dueLocalBalance != null) {
                resArr.push(dueLocalBalance);
            }

            let ledgerOpenAmount = this.getFinalObjectForExcelArray(myResponse.Result.LedgerOpenAmount, 'LedgerOpenAmount', ['$id']);
            if (ledgerOpenAmount != null) {
                resArr.push(ledgerOpenAmount);
            }


            let totalOpenReconciliationResult = this.getFinalObjectForExcelArray(myResponse.Result.TotalOpenReconciliationResult, 'TotalOpenReconciliationResult', ['$id']);
            if (totalOpenReconciliationResult != null) {
                resArr.push(totalOpenReconciliationResult);
            }

            let interestReportResult = this.getFinalObjectForExcelArray(myResponse.Result.InterestReportResult, 'InterestReportResult', ['$id']);
            if (interestReportResult != null) {
                resArr.push(interestReportResult);
            }

            const wb: XLSX.WorkBook = XLSX.utils.book_new();
            resArr.forEach(function (value) {
                let ws: XLSX.WorkSheet = XLSX.utils.json_to_sheet(value.array);
                ws['!cols'] = value?.wscols;
                XLSX.utils.book_append_sheet(wb, ws, value.name);
            });
            const now = new Date();
            let fileName = "AccountingIntegrityCheck-" + now.toLocaleDateString() + '.xlsx';
            XLSX.writeFile(wb, fileName);
        });
    }

    SetUIProperty() {
        this.UIProperties.SetEnabled("ResultXML", this.ObjectTableName, false);
        if (this.entityPM.Id && this.entityPM.Id != "new") {
            this.UIProperties.SetEnabled("FromMonthInclusive", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ToMonthInclusive", this.ObjectTableName, false);
        }
    }

    //#region Properties

    get Tenant() { return this._parameters.Tenant; }
    set Tenant(value: number) {
        if (this.entityPM.Tenant != value) {
            this.entityPM.Tenant = value;
        }
    }

    get FromMonthInclusive() { return this.entityPM.FromMonthInclusive; }
    set FromMonthInclusive(value: Date) {
        if (this.entityPM.FromMonthInclusive != value) {
            this.entityPM.FromMonthInclusive = value;
            // this.encodeParameters();

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }

        }
    }

    get ToMonthInclusive() { return this.entityPM.ToMonthInclusive; }
    set ToMonthInclusive(value: Date) {
        if (this.entityPM.ToMonthInclusive != value) {
            this.entityPM.ToMonthInclusive = value;
            // this.encodeParameters();

            if (!this.isValidate)
                this.validateDates();
            else {
                this.isValidate = false;
            }
        }
    }


    get ResultXML() { return this.entityPM.ResultXML; }
    set ResultXML(value: string) {
        if (this.entityPM.ResultXML != value) {
            this.entityPM.ResultXML = value;
        }
    }

    //#endregion


    //#region Date Filters Validation
    isValidate: boolean = false;
    validateDates() {
        setTimeout(() => {
            if (this.FromMonthInclusive > this.ToMonthInclusive) {

                this.UIProperties.SetValidity("ToMonthInclusive", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate"));
                this.UIProperties.SetValidity("FromMonthInclusive", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.FromDateMustSmallerToDate"));
                this.CD.detectChanges();

            } else {
                this.UIProperties.SetValidity("ToMonthInclusive", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromMonthInclusive", this.ObjectTableName, true, "");
                this.CD.detectChanges();

            }
        }, 200);
    }

    ReloadData() {
    }

    RunService() {
        this.Fixing = true;
        this.entityPM.StatusCode = "2";
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.AccountingIntegrityCheckPMService.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {


                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                    this.AccountingEntegrityCheckExtendedPMService.PostFixEntegrityCheckErrorInBatch(this.entityPM).subscribe((myResult: ServiceResponse) => {

                        this.CurrentSession.StopBusyIndicator();

                        var mm: ServiceResponse = myResult;
                        var entity = mm.Result;


                    });
                }

                else {

                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

    ViewResultXMLButtonClicked() {
        var _apiUrl = ServiceHelper.GetLogitudeURL() + 'api/AccountingEntegrityCheck';
        var url = _apiUrl + '/getAccountingIntegrityResultByIdAndTenant?' + 'id=' + this.entityPM.Id + '&tenant=' + this.entityPM.Tenant;
        var win = window.open(url, '_blank');

        if (win) {
            win.focus();
        }
    }
}

export class IntegrityCheckParameters {
    constructor(){}
    Tenant: number;
    FromMonthInclusive: Date;
    ToMonthInclusive: Date;
}
