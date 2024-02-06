import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import { Component, ChangeDetectorRef, ViewChildren, ViewChild, ViewContainerRef } from '@angular/core';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ServiceArgs } from '../../../Infrastructure/DataContracts/ServiceArgs';
import { OnInit, Output, EventEmitter, ComponentRef, QueryList } from '@angular/core';
import { JournalExtendedListService } from '../../Services/ExtendedLists/JournalExtendedListService';
import { ARPaymentExtendedListService } from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../Infrastructure/Tools';
import { ReconcileEventManager } from '../../Utilities/ReconcileEventManager';

import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ListComponentArgs } from 'Infrastructure/Args';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { LocationDirective } from 'Infrastructure/Utilities/LocationDirective';
import { ChildDirective } from 'Controls/Directives/ChildDirective';
import { GLAccountSecurityLevelService } from 'Accounting/Utilities/GLAccountSecurityLevelService';
@Component({

    templateUrl: "./GlAccountLedgerTransactionsListTemplate.html"
})
export class GlAccountLedgerTransactionsListTemplate {
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public Source: any;
    public IconCode: string;
    public ColorCode: string;
    public TenantCurrencySign: string;
    public ChequeStatusColor = "black";
    public ChartOfAccountsTypeCode: string;
    public ChartOfAccountsTypeBankCode = '5';
    public ChequeStatusColorDictionary = {
        'הופקד- טרם נפרע': 'orange',
        'בקופה': 'orange',
        'משמרת': 'orange',
        'הוחזר ללקוח': 'red',
        'נפרע': 'green',
    };


    public _JournalExtendedListService = new JournalExtendedListService();
    public _ARPaymentExtendedListService = new ARPaymentExtendedListService();
    @Output() CheckBoxChecked = new EventEmitter();
    @Output() Changed: EventEmitter<boolean> = new EventEmitter<boolean>();
    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    IsMultiWithReconcileMethodCodeEqualOne: boolean = false;
    reconcileEventManager:ReconcileEventManager;
    constructor(private CD: ChangeDetectorRef,) {
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        this.Listen();
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";

    }

    checkBoxState: boolean = false;
    get CheckBoxState() {
        return this.checkBoxState;
    }
    set CheckBoxState(isChecked: boolean) {
        console.log("Changed to: ", isChecked);
        this.checkBoxState = isChecked;
    }


    public get transferAccountId(): string {
        return this.CurrentSession.TransferAccountId;
    }


    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        if (this.rowData.IsChecked == true) {
            console.log("Oh Yea True");
        } else {
            console.log("Else " + this.rowData.IsChecked);
        }
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;
        this.reconcileEventManager = this.AdditionalData?.GridAdditionalData?.ReconcileEventManager;
        this.ChartOfAccountsTypeCode = MyAdditionalData;
        //#region Set Icons

        this.IconCode = AccountingEntityHelper.getEntityIcon(this.rowData.SourceTypeCode);
        if (GLAccountSecurityLevelService.IsMultiWithReconcileMethodCodeEqualOneParameter && !GLAccountSecurityLevelService.IsCheckBoxEnabledParameter) {
            this.IsCheckBoxEnabled = false;
        }
        //#endregion

        var isDestroyed: boolean = this.CD["destroyed"];
        if (!isDestroyed) { 
            this.CD.detectChanges();
        }
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    OpenSource(id: string) {
        // Type:    SourceTypeCode
        // Id:      SourceId
        // Display: SourceNumber

        var tableName = AccountingEntityHelper.getEntityObjectTableName(this.rowData.SourceTypeCode);

        SessionLocator.DynamicLoader.Load(
            "./Infrastructure/Components/EditComponent/EditComponent",
            this.CurrentSession.SessionLocation.viewContainerRef
        ).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName
            });

        });
    }

    async OpenTaxReportId(id: string) {
        SessionLocator.DynamicLoader.Load(
            "./Infrastructure/Components/EditComponent/EditComponent",
            this.CurrentSession.SessionLocation.viewContainerRef
        ).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: 'TaxReport'      
            });
        });
    }

    OpenManageReconciliations(rowData: any, title: string) {

        if (title != "סכום פתוח ") {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                .then(cmpRef => {

                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({
                        SelectedTabCode: "GAMR",
                        EntityId: rowData['AccountId'],
                        ObjectTableName: "GLAccount",
                        FromDate: new Date('01/01/2010'),
                        JournalNumber: rowData['JournalNumber']


                    });
                    cmpRef.instance.BackCompleted.subscribe(bk => {
                        console.log(bk);
                    });

                });
        }


    }

    private Listen() {
        GLAccountSecurityLevelService.IsCheckBoxEnabled.subscribe(($event) => {
            this.isCheckBoxEnabled = GLAccountSecurityLevelService.IsCheckBoxEnabledParameter;
            this.CD.detectChanges();
        });
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load(
                "./Infrastructure/Components/EditComponent/EditComponent",
                this.CurrentSession.SessionLocation.viewContainerRef
            ).then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: "Journal"
                });
                cmpRef.instance.BackCompleted.subscribe(bk => { });
            });
        }
    }

    CheckBoxClicked(checked: boolean) {
        //console.log("clicked: ", checked);
        //this.rowData['IsChecked'] = checked;

        if (this.IsCheckBoxEnabled) {
            this.reconcileEventManager.CheckBoxChecked.emit({
                line: this.rowData,
                isChecked: checked,
                RowIndex: this.AdditionalData.rowIndex
            });
        }
        //ReconcileEventManager.RowUnselected.subscribe(($event) => {
        //    this.rowData = ro
        //});
    }

    CalculateOriginalAmount() {
        if (
            !AppTool.IsNullOrEmpty(
                this.rowData["ReconcileMethodCode"]
            )
        ) {
            if (this.rowData["ReconcileMethodCode"] == "0") {
                // 0-local currency

                if (this.rowData["LocalAmountCredit"] == 0) {
                    return this.rowData["LocalAmountDebit"];
                } else {
                    return this.rowData["LocalAmountCredit"]; // -1 *
                }
            } else if (
                this.rowData["ReconcileMethodCode"] == "1"
            ) {
                // 1-foreign currency

                if (this.rowData["ForeignAmountCredit"] == 0) {
                    return this.rowData["ForeignAmountDebit"];
                } else {
                    return this.rowData["ForeignAmountCredit"];  // -1 *
                }
            }
        } else if (
            !AppTool.IsNullOrEmpty(
                this.reconcileEventManager.GLAccountReconcileMethodCode
            )
        ) {
            if (this.reconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency

                if (this.rowData["LocalAmountCredit"] == 0) {
                    return this.rowData["LocalAmountDebit"];
                } else {
                    return this.rowData["LocalAmountCredit"]; // -1 *
                }
            } else if (
                this.reconcileEventManager.GLAccountReconcileMethodCode == "1"
            ) {
                // 1-foreign currency

                if (this.rowData["ForeignAmountCredit"] == 0) {
                    return this.rowData["ForeignAmountDebit"];
                } else {
                    return this.rowData["ForeignAmountCredit"];  // -1 *
                }
            }
        }
    }

    CalculatOriginalCurruncy() {
        if (
            !AppTool.IsNullOrEmpty(
                this.reconcileEventManager.GLAccountReconcileMethodCode
            )
        ) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.reconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;
            } else if (
                this.reconcileEventManager.GLAccountReconcileMethodCode == "1"
            ) {
                // 1-foreign currency

                // foreign
                return this.rowData["CurrencySign"];
            }
        }
    }

    GetIndicatorText() {
        if (this.rowData['OpenAmount'] == 0) return this.showLocal ? "סגור" : "close";
        if ((this.rowData['LocalAmountDebit'] > 0 && this.rowData['OpenAmount'] != this.CalculateOriginalAmount()) || (this.rowData['LocalAmountCredit'] > 0 && this.rowData['OpenAmount'] != -1 * this.CalculateOriginalAmount()))
            return this.showLocal ? "סכום פתוח חלקית" : "Partial transaction";
        else return this.showLocal ? "סכום פתוח " : "Open transaction";
    }

    GetGLAccountIndicatorText() {
        if (this.rowData['OpenAmount'] == 0) return this.showLocal ? "סגור" : "close";
        if ((this.rowData['LocalAmountDebit'] != 0 && this.rowData['OpenAmount'] != this.CalculateOriginalAmount()) || (this.rowData['LocalAmountCredit'] != 0 && this.rowData['OpenAmount'] != -1 * this.CalculateOriginalAmount()))
            return this.showLocal ? "סכום פתוח חלקית" : "Partial transaction";
        else if (this.rowData['IsExternalReconcile'] == false && this.ChartOfAccountsTypeCode == "5") return this.showLocal ? "תנועות חיצוניות פתוחות " : "Open External Transaction";
        else return this.showLocal ? "סכום פתוח " : "Open transaction";
    }

    OpenGLAccount(fieldName: string) {
        var account2open = this.rowData[fieldName];
        var tableName = "GLAccount";

        SessionLocator.DynamicLoader.Load(
            "./Infrastructure/Components/EditComponent/EditComponent",
            this.CurrentSession.SessionLocation.viewContainerRef
        ).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: account2open,
                ObjectTableName: tableName
            });
        });
    }

    GetChequeStatusColor(chequeStatus) {

        this.ChequeStatusColor = this.ChequeStatusColorDictionary[chequeStatus];

        return this.ChequeStatusColor;
    }

    isCheckBoxEnabled: boolean = true;
    get IsCheckBoxEnabled() { return this.isCheckBoxEnabled; }
    set IsCheckBoxEnabled(value: boolean) {
        if (this.isCheckBoxEnabled != value) {
            this.isCheckBoxEnabled = value;
        }
    }

}
