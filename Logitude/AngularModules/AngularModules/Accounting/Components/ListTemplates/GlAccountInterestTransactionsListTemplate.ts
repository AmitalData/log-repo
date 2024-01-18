import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ARPaymentExtendedListService} from '../../../Invoice/Services/ExtendedLists/ARPaymentExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';

import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
@Component({

    templateUrl: "./GlAccountInterestTransactionsListTemplate.html"
})
export class GlAccountInterestTransactionsListTemplate {
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public Source: any;
    public IconCode: string;
    public ColorCode: string;
    public TenantCurrencySign: string;


    public journalExtendedListService = new JournalExtendedListService();
    public arPaymentExtendedListService = new ARPaymentExtendedListService();

    @Output() CheckBoxChecked = new EventEmitter();
    @Output() Changed: EventEmitter<boolean> = new EventEmitter<boolean>();

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.SetDefaultVariables();
    }

    private SetDefaultVariables()
    {
        this.TenantCurrencySign = SessionLocator.TenantPM.CurrencySign;
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        this.SetSourceIcon();

        var isDestroyed: boolean = this.CD["destroyed"];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    private SetSourceIcon()
    {
        this.IconCode = AccountingEntityHelper.getEntityIcon(this.rowData.SourceTypeCode);
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

    OpenSource(id: string)
    {
        const tableName = AccountingEntityHelper.getEntityObjectTableName(this.rowData.SourceTypeCode);
        this.OpenEntity(id, tableName);
    }

    OpenJournal(id) {
        this.OpenEntity(id, "Journal");
    }

    OpenInterestReport(id) {
        this.OpenEntity(id, "InterestReport");
    }

    private OpenEntity(id: any, tableName: string)
    {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load(
                "./Infrastructure/Components/EditComponent/EditComponent",
                this.CurrentSession.SessionLocation.viewContainerRef
            ).then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName
                });
                cmpRef.instance.BackCompleted.subscribe(bk => { });
            });
        }
    }
}
