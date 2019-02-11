import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ARPaymentExtendedListService} from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {JournalList} from '../../EntityLists/JournalList';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';

import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
@Component({
    moduleId: module.id,
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

    public _JournalExtendedListService = new JournalExtendedListService();
    public _ARPaymentExtendedListService = new ARPaymentExtendedListService();

    @Output() CheckBoxChecked = new EventEmitter();
    @Output() Changed: EventEmitter<boolean> = new EventEmitter<boolean>();

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;

    constructor(private CD: ChangeDetectorRef) {
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
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

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        if (this.rowData.IsChecked == true) {
            console.log("Oh Yea True");
        } else {
            console.log("Else " + this.rowData.IsChecked);
        }
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        //#region Set Icons

        this.IconCode = AccountingEntityHelper.getEntityIcon(this.rowData.SourceTypeCode);

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

        var tableName = "Journal";

        switch (this.rowData.SourceTypeCode) {
            // 1-Journal
            case "1": {
                tableName = "Journal";
                break;
            }

            // 2-ARInvoice
            case "2": {
                tableName = "ARInvoice";
                break;
            }

            // 3-ARPayment
            case "3": {
                tableName = "ARPayment";

                break;
            }

            // 4-APInvoice
            case "4": {
                tableName = "APInvoice";

                break;
            }

            // 5-APPayment
            case "5": {
                tableName = "APPayment";

                break;
            }

            // 6-Cheque Deposit
            case "6": {
                tableName = "BankDeposit";

                break;
            }

            // 7-Cash Deposit
            case "7": {
                tableName = "BankDeposit";

                break;
            }

            // 8-Revaluation
            case "8": {
                tableName = "Revaluation";

                break;
            }

            // 9-PaymentCheque
            case "9": {
                tableName = "PaymentCheque";

                break;
            }
        }

        SessionLocator.DynamicLoader.Load(
            "./Infrastructure/Components/EditComponent/EditComponent",
            SessionLocator.CurrentSession.SessionLocation.viewContainerRef
        ).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: id,
                ObjectTableName: tableName
            });
        });
    }

    OpenJournal(id) {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load(
                "./Infrastructure/Components/EditComponent/EditComponent",
                SessionLocator.CurrentSession.SessionLocation.viewContainerRef
            ).then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: "Journal"
                });
                cmpRef.instance.BackCompleted.subscribe(bk => {});
            });
        }
    }

    CheckBoxClicked(checked: boolean) {
        //console.log("clicked: ", checked);
        //this.rowData['IsChecked'] = checked;
        ReconcileEventManager.CheckBoxChecked.emit({
            line: this.rowData,
            isChecked: checked,
            RowIndex: this.AdditionalData.rowIndex
        });
        //ReconcileEventManager.RowUnselected.subscribe(($event) => {
        //    this.rowData = ro
        //});
    }

    CalculateOriginalAmount() {
        if (
            !AppTool.IsNullOrEmpty(
                ReconcileEventManager.GLAccountReconcileMethodCode
            )
        ) {
            if (ReconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency

                if (this.rowData["LocalAmountCredit"] == 0) {
                    return this.rowData["LocalAmountDebit"];
                } else {
                    return -1 * this.rowData["LocalAmountCredit"];
                }
            } else if (
                ReconcileEventManager.GLAccountReconcileMethodCode == "1"
            ) {
                // 1-foreign currency

                if (this.rowData["ForeignAmountCredit"] == 0) {
                    return this.rowData["ForeignAmountDebit"];
                } else {
                    return -1 * this.rowData["ForeignAmountCredit"];
                }
            }
        }
    }

    CalculatOriginalCurruncy() {
        if (
            !AppTool.IsNullOrEmpty(
                ReconcileEventManager.GLAccountReconcileMethodCode
            )
        ) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (ReconcileEventManager.GLAccountReconcileMethodCode == "0") {
                // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;
            } else if (
                ReconcileEventManager.GLAccountReconcileMethodCode == "1"
            ) {
                // 1-foreign currency

                // foreign
                return this.rowData["CurrencySign"];
            }
        }
    }

    GetIndicatorText() {
        if (this.rowData["OpenAmount"] != this.CalculateOriginalAmount())
            return this.showLocal ? "סכום פתוח חלקית" : "Partial transaction";
        else return this.showLocal ? "סכום פתוח " : "Open transaction";
    }

    OpenGLAccount() {
        var account2open = this.rowData["OppositeAccountId"];



        var tableName = "GLAccount";
        SessionLocator.DynamicLoader.Load(
            "./Infrastructure/Components/EditComponent/EditComponent",
            SessionLocator.CurrentSession.SessionLocation.viewContainerRef
        ).then(cmpRef => {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({
                EntityId: account2open,
                ObjectTableName: tableName
            });
        });
    }
}
